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
    private readonly IDashBoardService _dashBoardService;

    public HomeController(IClienteService clienteService, IProdutoService produtoService, IDashBoardService dashBoardService)
    {
        _clienteService = clienteService;
        _produtoService = produtoService;
        _dashBoardService = dashBoardService;
    }

    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var ultimoLogin = HttpContext.Session.GetString("UltimoLogin");
        var clientes = await _clienteService.ObterTodosEntidadeAsync();
        var produtos = await _produtoService.ObterTodosEntidadeAsync();

        DateTime parsedDate;
        var ultimoLoginDate = DateTime.TryParse(ultimoLogin, out parsedDate)
            ? parsedDate
            : DateTime.Now;

        var viewModel = new DashboardViewModel
        {
            TotalClientes = clientes.Count,
            TotalProdutos = produtos.Count,
            UltimoLogin = ultimoLoginDate
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObterDadosGrafico(string agrupamento)
    {
        if (string.IsNullOrEmpty(agrupamento))
            agrupamento = "ano";

        var dados = await _dashBoardService.ObterDadosGraficoAsync(agrupamento);

        return Ok(dados);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Logout()
    {
        return RedirectToAction("Logout", "Account");
    }
}
