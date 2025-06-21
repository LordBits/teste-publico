using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Services;
using Teste.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Força HTTPS nos cookies
    });

// Add services to the container
builder.Services.AddControllersWithViews();

// Sessão (precisa antes do app.Build)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependências
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Pipeline de middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

// BLOQUEIO DE CACHE EM PÁGINAS AUTENTICADAS
app.Use(async (context, next) =>
{
    await next();

    // Só tenta mexer nos headers se a resposta NÃO tiver começado ainda
    if (!context.Response.HasStarted
        && context.User.Identity != null
        && context.User.Identity.IsAuthenticated
        && context.Response.ContentType != null
        && context.Response.ContentType.Contains("text/html"))
    {
        context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
    }
});

// Autenticação e Autorização (se for usar futuramente)
app.UseAuthentication();
app.UseAuthorization();

// Sessão (sempre depois do Routing e antes do Endpoint Mapping)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();