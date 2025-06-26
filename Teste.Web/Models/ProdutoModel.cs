using System.ComponentModel.DataAnnotations;

namespace Teste.Web.Models
{
    public class ProdutoModel
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Código de barras é obrigatório.")]
        [Display(Name = "Código de barras")]
        public string CodigoBarra { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor de Venda é obrigatório.")]
        [Display(Name = "Valor de Venda em R$")]
        public string ValorVenda { get; set; } = string.Empty;

        [Required(ErrorMessage = "Peso Bruto é obrigatório.")]
        [Display(Name = "Peso Bruto em Kg")]
        public string PesoBruto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Peso Líquido é obrigatório.")]
        [Display(Name = "Peso Líquido em Kg")]
        public string PesoLiquido { get; set; } = string.Empty;
    }
}