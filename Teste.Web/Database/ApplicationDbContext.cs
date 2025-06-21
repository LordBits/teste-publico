using Microsoft.EntityFrameworkCore;
using Teste.Web.Models;

namespace Teste.Web.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para a tabela Produto - Ajustando o tipo do campo decimal Preco
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");

            // Se no futuro precisar adicionar mais configurações de tabelas (constraints, índices, etc)
            // modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
        }
    }
}