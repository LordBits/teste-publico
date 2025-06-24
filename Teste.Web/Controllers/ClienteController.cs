using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Core;
using Teste.Web.Models;
using Teste.Web.Services;

namespace Teste.Web.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var clientes = await _clienteService.ObterTodosAsync(pageNumber, pageSize);

            return View(clientes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ClienteModel(){Codigo = 0, Nome = "", Fantasia = "", Documento = "", Endereco = "" };

            return View("Cliente", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            var model = _mapper.Map<ClienteModel>(cliente);

            return View("Cliente", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ClienteModel clienteModel)
        {
            if (!ModelState.IsValid)
                return View("Cliente", clienteModel);

            try
            {
                var cliente = _mapper.Map<Cliente>(clienteModel);

                if (cliente.Codigo == 0)
                    await _clienteService.SalvarAsync(cliente);
                else
                    await _clienteService.SalvarAsync(cliente);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao salvar cliente: {ex.Message}");

                return View("Cliente", clienteModel);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.DeletarAsync(id);

                return Json(new { success = true, message = "Cliente deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao deletar: {ex.Message}" });
            }
        }

        //Visualizar cliente via PartialView para modal
        [HttpGet]
        public async Task<IActionResult> Visualizar(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);

            if (cliente == null)
                return NotFound();

            var clienteModel = _mapper.Map<ClienteModel>(cliente);

            return PartialView("_VisualizarCliente", clienteModel);
        }
    }
}