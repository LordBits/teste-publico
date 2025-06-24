using Teste.Web.Core;
using Teste.Web.Models;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public interface IClienteService
    {
        Task<PagedResult<ClienteModel>> ObterTodosAsync(int pageNumber, int pageSize);
        Task<int> ContarClientesAsync();
        Task DeletarAsync(int id);
        Task<Cliente> ObterPorIdAsync(int clienteId);
        Task SalvarAsync(Cliente cliente);
    }
}