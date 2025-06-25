using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teste.Web.Core;
using Teste.Web.Models;
using Teste.Web.Services;

namespace Teste.Web.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutoController(IProdutoService produtoService, IMapper mapper)
        {
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var produtos = await _produtoService.ObterTodosAsync(pageNumber, pageSize);

            return View(produtos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProdutoModel();

            return View("Produto", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);

            if (produto == null)
                return NotFound();

            var model = _mapper.Map<ProdutoModel>(produto);

            return View("Produto", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProdutoModel produtoModel)
        {
            if (!ModelState.IsValid)
                return View("Produto", produtoModel);

            var isNewProduto = produtoModel.Codigo == 0;

            try
            {
                var produto = _mapper.Map<Produto>(produtoModel);

                await _produtoService.SalvarAsync(produto);

                TempData["Message"] = isNewProduto ? "Produto cadastrado com sucesso!" : "Produto atualizado com sucesso!";

                TempData["tipoAlert"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Erro ao salvar produto: {ex.Message}";

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
                await _produtoService.DeletarAsync(id);

                return Json(new { success = true, message = "Produto deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erro ao deletar: {ex.Message}" });
            }
        }

        //Visualizar produto via PartialView para modal
        [HttpGet]
        public async Task<IActionResult> Visualizar(int id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);

            if (produto == null)
                return NotFound();

            var produtoModel = _mapper.Map<ProdutoModel>(produto);

            return PartialView("_VisualizarProduto", produtoModel);
        }
    }
}