using System.ComponentModel.DataAnnotations;

namespace Teste.Web.Models
{
    public class ClienteModel
    {
        public int Codigo { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fantasia é obrigatório")]
        public string Fantasia { get; set; } = string.Empty;

        [Required(ErrorMessage = "Documento é obrigatório")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Endereço é obrigatório")]
        public string Endereco { get; set; } = string.Empty;
    }
}