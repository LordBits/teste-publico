var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

    // Em produção, sempre usar HTTPS
    app.UseHttpsRedirection();
}
else
{
    // Em desenvolvimento, só usar HTTPS se a porta HTTPS estiver configurada
    var httpsPort = app.Urls
        .Select(url => new Uri(url))
        .FirstOrDefault(u => u.Scheme == Uri.UriSchemeHttps)?.Port;

    if (httpsPort.HasValue)
    {
        app.UseHttpsRedirection();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();