using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Models;
using Teste.Web.Interfaces;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Cliente>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var query = _context.Clientes.AsQueryable();

            var totalItems = await query.CountAsync();

            var items = await query
                    .OrderBy(c => c.Codigo)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagedResult<Cliente>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> ContarClientesAsync()
        {
            return await _context.Clientes.CountAsync();
        }
    }
}