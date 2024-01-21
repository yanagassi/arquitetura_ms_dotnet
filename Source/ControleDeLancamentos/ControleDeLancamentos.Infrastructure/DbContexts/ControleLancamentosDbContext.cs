using Microsoft.EntityFrameworkCore;


namespace ControleDeLancamentos.Infrastructure.DbContexts
{
    public class ControleLancamentosDbContext : DbContext
    {
        public ControleLancamentosDbContext(DbContextOptions<ControleLancamentosDbContext> options) : base(options)
        {
        }


        public DbSet<Domain.Entities.ContaBancaria> ContasBancarias { get; set; }
        public DbSet<Domain.Entities.Lancamento> Lancamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                      
            modelBuilder.Entity<Domain.Entities.Lancamento>(entity =>
            { 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Valor).IsRequired();
                entity.Property(e => e.Tipo).IsRequired();
                entity.Property(e => e.Descricao).IsRequired();
                entity.Property(e => e.Data).IsRequired();

                entity.HasOne(e => e.ContaBancaria)
                    .WithMany(c => c.Lancamentos)
                    .HasForeignKey(e => e.ContaId)
                    .OnDelete(DeleteBehavior.Cascade);  
            });


        }
    }
}
