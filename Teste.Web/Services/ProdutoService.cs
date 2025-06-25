using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teste.Web.Core;
using Teste.Web.Database;
using Teste.Web.Models;
using Teste.Web.Models.ViewModels;

namespace Teste.Web.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProdutoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProdutoModel>> ObterTodosAsync(int pageNumber, int pageSize)
        {
             var query = _context.Produtos.AsQueryable();

            var totalItems = await query.CountAsync();

            // Calcula total de páginas
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Se a página solicitada é maior do que a última, volta uma página
            if (pageNumber > totalPages && totalPages > 0)
            {
                pageNumber = totalPages;
            }

            var items = await query
                .OrderBy(c => c.Codigo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProdutoModel>
            {
                Items = items.Select(_mapper.Map<ProdutoModel>).ToList(),
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> ContarProdutosAsync()
        {
            return await _context.Produtos.CountAsync();
        }

        public async Task DeletarAsync(int produtoId)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);

            if (produto == null)
                throw new Exception("Produto não encontrado.");

            _context.Produtos.Remove(produto);

            await _context.SaveChangesAsync();
        }

        public async Task<Produto> ObterPorIdAsync(int produtoId)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);

            if (produto == null)
                throw new Exception("Produto não encontrado.");

            return produto;
        }

        public async Task SalvarAsync(Produto produto)
        {
            if (produto.Codigo == 0)
            {
                await _context.Produtos.AddAsync(produto);
            }
            else
            {
                _context.Produtos.Update(produto);
            }

            await _context.SaveChangesAsync();
        }
    }
}