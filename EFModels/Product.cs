using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Product
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int TillandsiaId { get; set; }

    public decimal Price { get; set; }

    public string? Size { get; set; }

    public int? Quantity { get; set; }
    
    public Company Company { get; set; } = default!;

    public Tillandsia Tillandsia { get; set; } = default!;
}

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(p => p.Id)
            .IsRequired()
            .HasColumnOrder(0);

        builder
            .Property(p => p.CompanyId)
            .IsRequired()
            .HasColumnName("Company_Id")
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder
            .Property(p => p.TillandsiaId)
            .IsRequired()
            .HasColumnName("Tillandsia_Id")
            .HasColumnType("int")
            .HasColumnOrder(2);

        builder
            .Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(6,3)")
            .HasColumnOrder(3);

        builder
            .Property(p => p.Size)
            .HasColumnType("varchar(20)")
            .HasColumnOrder(4);

        builder
            .Property(p => p.Quantity)
            .HasColumnType("int")
            .HasColumnOrder(5);

        builder
            .HasOne(p => p.Company)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ForeignKey_CompanyId");

        builder
            .HasOne(p => p.Tillandsia)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.TillandsiaId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ForeignKey_TillandsiaId");
    }
}