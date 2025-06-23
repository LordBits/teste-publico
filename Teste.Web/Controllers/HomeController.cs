using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Interfaces;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IProdutoService _produtoService;
    private readonly IUsuarioService _usuarioService;

    public HomeController(IClienteService clienteService, IProdutoService produtoService, IUsuarioService usuarioService)
    {
        _clienteService = clienteService;
        _produtoService = produtoService;
        _usuarioService = usuarioService;
    }

    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int? userId = null;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (int.TryParse(userIdClaim, out int parsedId))
        {
            userId = parsedId;
        }

        DateTime? ultimoLogin = null;

        if (userId.HasValue)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(userId.Value);

            ultimoLogin = usuario?.UltimoLogin;
        }

        var totalClientes = await _clienteService.ContarClientesAsync();
        var totalProdutos = await _produtoService.ContarProdutosAsync();

        var viewModel = new DashboardViewModel
        {
            TotalClientes = totalClientes,
            TotalProdutos = totalProdutos,
            UltimoLogin = ultimoLogin ?? DateTime.UtcNow
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
