using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Models;
using Teste.Web.Interfaces;

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

            if (usuario == null)
                return null;

            return BCrypt.Net.BCrypt.Verify(password, usuario.SenhaHash) ? usuario : null;
        }
    }
}