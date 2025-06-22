namespace Teste.Web.Models
{
    public class Produto
    {
        public int Codigo { get; set; }
        public required string Descricao { get; set; }
        public required string CodigoBarra { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal PesoBruto { get; set; }
        public decimal PesoLiquido { get; set; }
    }
}