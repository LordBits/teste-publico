using Teste.Web.Core;

namespace Teste.Web.Services
{
    public interface IAuthService
    {
        Task<Usuario?> ValidateUserAsync(string email, string password);
    }
}