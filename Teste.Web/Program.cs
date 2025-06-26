using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Routing;
using Teste.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // Adiciona detalhes ao erro
);

builder.Services.AddHttpContextAccessor();

// Autenticação via Cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/account/login";
        options.LogoutPath = "/account/logout";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;          // Só HTTPS
        options.Cookie.HttpOnly = true;                                    // Não acessível via JS
        options.Cookie.SameSite = SameSiteMode.Strict;                    // Bloqueia envio em requisições externas
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

// Controle de rotas com slugify (para entrada e geração de URLs)
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// Configura transformação de saída (URLs geradas por RedirectToAction, TagHelpers etc.)
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
});

// Sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Swagger (ambiente de desenvolvimento)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependências
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddAutoMapper(typeof(Program));

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LoginLimiter", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5, // Máximo 5 tentativas
                Window = TimeSpan.FromMinutes(1), // Por minuto
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0 // Sem fila
            }));
});

builder.Services.Configure<MvcOptions>(options =>
{
    var provider = options.ModelBindingMessageProvider;

    provider.SetValueMustBeANumberAccessor(_ => "O campo deve ser um número.");
    provider.SetMissingBindRequiredValueAccessor(_ => "Campo obrigatório.");
    provider.SetMissingKeyOrValueAccessor(() => "Campo obrigatório.");
    provider.SetAttemptedValueIsInvalidAccessor((value, field) => $"O valor '{value}' não é válido para {field}.");
    provider.SetValueIsInvalidAccessor(_ => "Valor inválido.");
    provider.SetValueMustNotBeNullAccessor(_ => "O campo é obrigatório.");
});

var app = builder.Build();

//define a cultura global
var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Pipeline de middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/home/error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Content-Security-Policy",
        $"default-src 'self'; " +
        $"script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
        $"style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
        $"font-src 'self' https://cdn.jsdelivr.net; " +
        $"img-src 'self' data:;");

    await next();
});

app.UseRouting();

// Sessão (sempre depois do Routing e antes do Authentication)
app.UseSession();

// Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

// Bloqueio de cache em páginas autenticadas (IMPORTANTE: Sempre antes de endpoints)
app.Use(async (context, next) =>
{
    await next();

    if (!context.Response.HasStarted &&
        context.User.Identity != null &&
        context.User.Identity.IsAuthenticated &&
        context.Response.ContentType != null &&
        context.Response.ContentType.Contains("text/html"))
    {
        context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
    }
});

//Redirecionamento para raíz
app.MapGet("/", context =>
{
    context.Response.Redirect("/account/login");
    return Task.CompletedTask;
});

// Map de rotas (com slugify ativo)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller:slugify}/{action:slugify}/{id?}");

app.Run();