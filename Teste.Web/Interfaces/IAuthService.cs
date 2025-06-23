using Teste.Web.Models;

namespace Teste.Web.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario?> ValidateUserAsync(string email, string password);
    }
}