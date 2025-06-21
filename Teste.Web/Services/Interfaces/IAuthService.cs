using Teste.Web.Models;

namespace Teste.Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario?> ValidateUserAsync(string email, string password);
    }
}