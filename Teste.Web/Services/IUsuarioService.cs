using Teste.Web.Core;

namespace Teste.Web.Services
{
    public interface IUsuarioService
    {
        Task SalvarAsync(Usuario usuario);
        Task<Usuario?> ObterPorIdAsync(int usuarioId);
    }
}