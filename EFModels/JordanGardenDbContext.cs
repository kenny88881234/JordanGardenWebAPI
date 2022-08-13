using Microsoft.EntityFrameworkCore;

public class JordanGardenStockDbContext : DbContext
{
    public JordanGardenStockDbContext(DbContextOptions<JordanGardenStockDbContext> options) : base(options) { }
    public DbSet<Tillandsia> Tillandsias { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TillandsiaEntityTypeConfiguration).Assembly);
    }
}