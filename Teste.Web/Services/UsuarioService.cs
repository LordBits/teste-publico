using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Interfaces;
using Teste.Web.Models;

namespace Teste.Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(Usuario usuario)
        {
            if (usuario.Id == 0)
                _context.Usuarios.Add(usuario);
            else
                _context.Usuarios.Update(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(int usuarioId)
        {
            return await _context.Usuarios.SingleOrDefaultAsync(x => x.Id == usuarioId);
        }
    }
}