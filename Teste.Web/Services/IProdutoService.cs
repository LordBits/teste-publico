using Teste.Web.Core;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public interface IProdutoService
    {
        Task<PagedResult<Produto>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<int> ContarProdutosAsync();
    }
}