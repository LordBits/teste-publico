using Teste.Web.Models;

namespace Teste.Web.Services
{
    public interface IDashBoardService
    {
        Task<GraficoHomeModel> ObterDadosGraficoAsync(string agrupamento);
    }
}