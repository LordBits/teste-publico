using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Routing;
using Teste.Web.Services;
using Teste.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticação via Cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/account/login";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Força HTTPS nos cookies
    });

// Controle de rotas com slugify (para entrada e geração de URLs)
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
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

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

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

// Map de rotas (com slugify ativo)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller:slugify}/{action:slugify}/{id?}");

app.Run();