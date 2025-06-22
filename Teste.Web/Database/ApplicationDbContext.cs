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

            //Minhas chaves primárias com incremento
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.Codigo);
                entity.Property(c => c.Codigo).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(p => p.Codigo);
                entity.Property(p => p.Codigo).ValueGeneratedOnAdd();
            });

            // Configuração para a tabela Produto - Ajustando o tipo do campo decimal
            modelBuilder.Entity<Produto>().Property(p => p.ValorVenda).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Produto>().Property(p => p.PesoBruto).HasColumnType("decimal(18,3)");
            modelBuilder.Entity<Produto>().Property(p => p.PesoLiquido).HasColumnType("decimal(18,3)");
        }
    }
}