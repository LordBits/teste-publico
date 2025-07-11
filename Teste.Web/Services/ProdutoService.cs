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

        public async Task<PagedResult<ProdutoModel>> ObterTodosAsync(int pageNumber, int pageSize, string? busca = null)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                busca = busca.ToLower();

                query = query.Where(p =>
                    (!string.IsNullOrEmpty(p.Descricao) && p.Descricao.ToLower().Contains(busca)) ||
                    (!string.IsNullOrEmpty(p.CodigoBarra) && p.CodigoBarra.Contains(busca))
                );
            }

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

        public async Task IsCodigoBarraDuplicadoAsync(string codigoBarra, int? id)
        {
            var produto = await _context.Produtos
            .AsNoTracking() // Para não rastrear, evitar de duplicidade em transações do meu ID.
            .FirstOrDefaultAsync(p => p.CodigoBarra == codigoBarra);

            if ((produto != null && id == null) || (id != null && produto != null && produto.Codigo != id))
                throw new Exception("Não é possível cadastrar produtos diferentes com o mesmo código de barra.");
        }

        public async Task<IList<Produto>> ObterTodosEntidadeAsync()
        {
            return await _context.Produtos.ToListAsync();
        }
    }
}