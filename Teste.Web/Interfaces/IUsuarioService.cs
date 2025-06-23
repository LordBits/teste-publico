using Teste.Web.Models;

namespace Teste.Web.Interfaces
{
    public interface IUsuarioService
    {
        Task SalvarAsync(Usuario usuario);
        Task<Usuario?> ObterPorIdAsync(int usuarioId);
    }
}