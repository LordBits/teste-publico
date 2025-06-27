namespace Teste.Web.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }
        public int TotalProdutos { get; set; }
        public DateTime UltimoLogin { get; set; }
        public List<string> Meses { get; set; } = new();
        public List<int> ClientesPorMes { get; set; } = new();
        public List<int> ProdutosPorMes { get; set; } = new();
        public List<string> Anos { get; set; } = new();
        public List<int> ClientesPorAno { get; set; } = new();
        public List<int> ProdutosPorAno { get; set; } = new();
    }
}