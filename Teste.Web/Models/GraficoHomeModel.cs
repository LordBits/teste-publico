namespace Teste.Web.Models
{
    public class GraficoHomeModel
    {
        public List<string> Labels { get; set; } = new();
        public List<int> Clientes { get; set; } = new();
        public List<int> Produtos { get; set; } = new();
    }
}