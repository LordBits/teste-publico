namespace Teste.Web.Core
{
    public class Cliente
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Fantasia { get; set; }
        public required string Documento { get; set; }
        public required string Endereco { get; set; }
    }
}