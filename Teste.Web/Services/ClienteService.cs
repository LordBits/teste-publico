using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Models.ViewModels;
using Teste.Web.Core;
using Teste.Web.Models;
using AutoMapper;

namespace Teste.Web.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ClienteService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<ClienteModel>> ObterTodosAsync(int pageNumber, int pageSize)
        {
            var query = _context.Clientes.AsQueryable();

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

            return new PagedResult<ClienteModel>
            {
                Items = items.Select(_mapper.Map<ClienteModel>).ToList(),
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<int> ContarClientesAsync()
        {
            return await _context.Clientes.CountAsync();
        }

        public async Task DeletarAsync(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);

            if (cliente == null)
                throw new Exception("Cliente não encontrado.");

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();
        }

        public async Task<Cliente> ObterPorIdAsync(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);

            if (cliente == null)
                throw new Exception("Cliente não encontrado.");

            return cliente;
        }

        public async Task SalvarAsync(Cliente cliente)
        {
            if (cliente.Codigo == 0)
            {
                await _context.Clientes.AddAsync(cliente);
            }
            else
            {
                _context.Clientes.Update(cliente);
            }

            await _context.SaveChangesAsync();
        }

        public async Task IsClienteDuplicadoAsync(string documento, int? id)
        {
            var cliente = await _context.Clientes
            .AsNoTracking() // Para não rastrear, evitar de duplicidade em transações do meu ID.
            .FirstOrDefaultAsync(c => c.Documento == documento);

            if (cliente == null || cliente.Codigo == id)
                return;

            throw new Exception($"Este documento já está sendo utilizado por outro cliente.");
        }
    }
}