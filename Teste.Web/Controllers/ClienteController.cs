using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Interfaces;

namespace Teste.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [Authorize]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var clientes = await _clienteService.ObterTodosAsync(pageNumber, pageSize);

            return View(clientes);
        }
    }
}