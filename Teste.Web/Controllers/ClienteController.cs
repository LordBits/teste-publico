using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Core;
using Teste.Web.Helpers;
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
            var model = new ClienteModel();

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

            var documentoValidar = clienteModel.Documento;

            if (!DocumentoHelper.DocumentoEhValido(DocumentoHelper.SomenteNumeros(documentoValidar)))
            {
                ModelState.AddModelError(nameof(documentoValidar), "Documento inválido (CPF/CNPJ)");

                return View("Cliente", clienteModel);
            }

            var isNewCliente = clienteModel.Codigo == 0;

            try
            {
                var cliente = _mapper.Map<Cliente>(clienteModel);

                cliente.Documento = DocumentoHelper.SomenteNumeros(cliente.Documento);

                await _clienteService.IsClienteDuplicadoAsync(cliente.Documento, cliente.Codigo);

                await _clienteService.SalvarAsync(cliente);

                TempData["Message"] = isNewCliente ? "Cliente cadastrado com sucesso!" : "Cliente atualizado com sucesso!";

                TempData["tipoAlert"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao salvar cliente: {ex.Message}";

                TempData["tipoAlert"] = "danger";
            }

            return RedirectToAction("Index");
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