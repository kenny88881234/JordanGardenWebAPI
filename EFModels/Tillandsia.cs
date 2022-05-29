using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Tillandsia
{
    public int Id { get; set; }

    public string NameEng { get; set; } = default!;

    public string? NameChi { get; set; }

    public string? Image { get; set; }

    public List<Product> Products { get; set; } = default!;
}

public class TillandsiaEntityTypeConfiguration : IEntityTypeConfiguration<Tillandsia>
{
    public void Configure(EntityTypeBuilder<Tillandsia> builder)
    {
        builder
            .Property(t => t.Id)
            .IsRequired()
            .HasColumnOrder(0);

        builder
            .Property(t => t.NameEng)
            .IsRequired()
            .HasColumnName("Name_Eng")
            .HasColumnType("varchar(50)")
            .HasColumnOrder(1);

        builder
            .Property(t => t.NameChi)
            .HasColumnName("Name_Chi")
            .HasColumnType("nvarchar(20)")
            .HasColumnOrder(2);

        builder
            .Property(t => t.Image)
            .HasColumnName("Name_Eng")
            .HasColumnType("nvarchar(300)")
            .HasColumnOrder(3);
    }
}