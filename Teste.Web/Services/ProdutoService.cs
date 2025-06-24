using Microsoft.EntityFrameworkCore;
using Teste.Web.Core;
using Teste.Web.Database;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;
        public ProdutoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Produto>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var query = _context.Produtos.AsQueryable();

            var totalItems = await query.CountAsync();

            var items = await query
                    .OrderBy(c => c.Codigo)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagedResult<Produto>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> ContarProdutosAsync()
        {
            return await _context.Produtos.CountAsync();
        }
    }
}