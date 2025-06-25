using System.ComponentModel.DataAnnotations;

namespace Teste.Web.Models
{
    public class ProdutoModel
    {
        public int Codigo { get; set; }
        [Required(ErrorMessage = "Descrição é obrigatório")]
        public string Descricao { get; set; } = string.Empty;
        [Required(ErrorMessage = "Código de barra é obrigatório")]
        public string CodigoBarra { get; set; } = string.Empty;
        [Required(ErrorMessage = "Valor de venda é obrigatório")]
        public decimal ValorVenda { get; set; }
        [Required(ErrorMessage = "Peso bruto é obrigatório")]
        public decimal PesoBruto { get; set; }
        [Required(ErrorMessage = "Peso líquido é obrigatório")]
        public decimal PesoLiquido { get; set; }
    }
}