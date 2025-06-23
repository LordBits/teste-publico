using Teste.Web.Models;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Interfaces
{
    public interface IProdutoService
    {
        Task<PagedResult<Produto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<int> ContarProdutosAsync();
    }
}