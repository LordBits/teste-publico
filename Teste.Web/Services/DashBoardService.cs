using Microsoft.EntityFrameworkCore;
using Teste.Web.Database;
using Teste.Web.Models;

namespace Teste.Web.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ApplicationDbContext _context;

        public DashBoardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GraficoHomeModel> ObterDadosGraficoAsync(string agrupamento)
        {
            var dto = new GraficoHomeModel();

            if (agrupamento == "ano")
            {
                var dadosClientesAno = await _context.Clientes
                    .GroupBy(c => c.DataCadastro.Year)
                    .Select(g => new { Ano = g.Key, Total = g.Count() })
                    .OrderBy(x => x.Ano)
                    .ToListAsync();

                var dadosProdutosAno = await _context.Produtos
                    .GroupBy(p => p.DataCadastro.Year)
                    .Select(g => new { Ano = g.Key, Total = g.Count() })
                    .OrderBy(x => x.Ano)
                    .ToListAsync();

                var anos = dadosClientesAno.Select(x => x.Ano.ToString()).ToList();

                dto.Labels = anos;
                dto.Clientes = dadosClientesAno.Select(x => x.Total).ToList();
                dto.Produtos = dadosProdutosAno.Select(x => x.Total).ToList();
            }
            else
            {
                var anoAtual = DateTime.Now.Year;

                var dadosClientesMes = await _context.Clientes
                    .Where(c => c.DataCadastro.Year == anoAtual)
                    .GroupBy(c => c.DataCadastro.Month)
                    .Select(g => new { Mes = g.Key, Total = g.Count() })
                    .OrderBy(x => x.Mes)
                    .ToListAsync();

                var dadosProdutosMes = await _context.Produtos
                    .Where(p => p.DataCadastro.Year == anoAtual)
                    .GroupBy(p => p.DataCadastro.Month)
                    .Select(g => new { Mes = g.Key, Total = g.Count() })
                    .OrderBy(x => x.Mes)
                    .ToListAsync();

                dto.Labels = dadosClientesMes.Select(x =>
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Mes)).ToList();

                dto.Clientes = dadosClientesMes.Select(x => x.Total).ToList();
                dto.Produtos = dadosProdutosMes.Select(x => x.Total).ToList();
            }

            return dto;
        }
    }
}