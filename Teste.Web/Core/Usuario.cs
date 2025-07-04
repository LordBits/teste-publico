namespace Teste.Web.Core
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimoLogin { get; set; }
    }
}