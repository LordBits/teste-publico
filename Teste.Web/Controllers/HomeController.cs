using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Teste.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [HttpGet]
    public IActionResult Index()
    {
        var nome = HttpContext.Session.GetString("UsuarioNome");
        ViewBag.UsuarioNome = nome;

        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        return RedirectToAction("Logout", "Account");
    }
}
