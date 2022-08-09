using Microsoft.EntityFrameworkCore;

public class JordanGardenDbContext : DbContext
{
    public JordanGardenDbContext(DbContextOptions<JordanGardenDbContext> options) : base(options) { }
    public DbSet<Tillandsia> Tillandsias { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TillandsiaEntityTypeConfiguration).Assembly);
    }
}