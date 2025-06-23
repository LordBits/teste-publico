using Teste.Web.Models;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Interfaces
{
    public interface IClienteService
    {
        Task<PagedResult<Cliente>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<int> ContarClientesAsync();
    }
}