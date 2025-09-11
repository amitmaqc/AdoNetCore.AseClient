using EntityFrameworkCore.Ase.Internal;
using Microsoft.EntityFrameworkCore;

public class AseDbContext(DbContextOptions<AseDbContext> options) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DummyEntity>()
            .ToTable("DummyEntities")
            .Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .HasAnnotation("Ase:ValueGenerationStrategy", AseValueGenerationStrategy.IdentityColumn);
    }

    public required DbSet<DummyEntity> DummyEntities { get; set; }
}