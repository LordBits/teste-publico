using System.ComponentModel.DataAnnotations;

namespace Teste.Web.Models
{
    public class ClienteModel
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome completo")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome fantasia é obrigatório")]
        [Display(Name = "Nome fantasia")]
        public string Fantasia { get; set; } = string.Empty;

        [Required(ErrorMessage = "O documento é obrigatório")]
        [Display(Name = "CPF ou CNPJ")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório")]
        [Display(Name = "Endereço completo")]
        public string Endereco { get; set; } = string.Empty;
    }
}