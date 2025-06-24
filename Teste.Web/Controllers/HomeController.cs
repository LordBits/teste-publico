using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Models.ViewModels;
using Teste.Web.Services;

namespace Teste.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IProdutoService _produtoService;

    public HomeController(IClienteService clienteService, IProdutoService produtoService)
    {
        _clienteService = clienteService;
        _produtoService = produtoService;
    }

    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var ultimoLogin = HttpContext.Session.GetString("UltimoLogin");
        var totalClientes = await _clienteService.ContarClientesAsync();
        var totalProdutos = await _produtoService.ContarProdutosAsync();

        DateTime parsedDate;
        var ultimoLoginDate = DateTime.TryParse(ultimoLogin, out parsedDate)
            ? parsedDate
            : DateTime.UtcNow;

        var viewModel = new DashboardViewModel
        {
            TotalClientes = totalClientes,
            TotalProdutos = totalProdutos,
            UltimoLogin = ultimoLoginDate
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        return RedirectToAction("Logout", "Account");
    }
}
