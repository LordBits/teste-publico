using Teste.Web.Core;
using Teste.Web.Models;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public interface IProdutoService
    {
        Task<PagedResult<ProdutoModel>> ObterTodosAsync(int pageNumber, int pageSize, string? busca = null);
        Task DeletarAsync(int id);
        Task<Produto> ObterPorIdAsync(int produtoId);
        Task SalvarAsync(Produto produto);
        Task IsCodigoBarraDuplicadoAsync(string codigoBarra, int? id);
        Task<IList<Produto>> ObterTodosEntidadeAsync();
    }
}