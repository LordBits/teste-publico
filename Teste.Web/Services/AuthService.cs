using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Core;

namespace Teste.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ValidateUserAsync(string email, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());

            // Hash fake para evitar timing attack
            const string fakeHash = "$2a$12$Wq3v2Eo7eYzXf1JZxTq0SOuLd5PrX5kIQm9kzv3iZ6PsM/OavBq0e"; // bcrypt de senha qualquer

            var hashToCheck = usuario?.SenhaHash ?? fakeHash;

            return BCrypt.Net.BCrypt.Verify(password, hashToCheck) ? usuario : null;
        }
    }
}