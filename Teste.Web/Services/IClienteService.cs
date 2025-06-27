using Teste.Web.Core;
using Teste.Web.Models;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public interface IClienteService
    {
        Task<PagedResult<ClienteModel>> ObterTodosAsync(int pageNumber, int pageSize, string? busca = null);
        Task DeletarAsync(int id);
        Task<Cliente> ObterPorIdAsync(int clienteId);
        Task SalvarAsync(Cliente cliente);
        Task IsClienteDuplicadoAsync(string documento, int? id);
        Task<IList<Cliente>> ObterTodosEntidadeAsync();
    }
}