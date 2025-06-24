namespace Teste.Web.Models
{
    public class ClienteModel
    {
        public ClienteModel()
        {     
        }

        public int Codigo { get; set; }
        public required string Nome { get; set; } = string.Empty;
        public required string Fantasia { get; set; } = string.Empty;
        public required string Documento { get; set; } = string.Empty;
        public required string Endereco { get; set; } = string.Empty;
    }
}