using Microsoft.EntityFrameworkCore;

public class AutenticacaoDbContext : DbContext
{
    public AutenticacaoDbContext(DbContextOptions<AutenticacaoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(u => u.Id).IsRequired();
            entity.Property(u => u.Nome).IsRequired();
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.SenhaHash).IsRequired();
        });
    }
}
