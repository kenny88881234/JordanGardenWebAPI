using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Tillandsia
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nameEng")]
    public string NameEng { get; set; } = default!;

    [JsonPropertyName("nameChi")]
    public string? NameChi { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonIgnore]
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
            .HasColumnType("varchar(20)")
            .HasColumnOrder(2);

        builder
            .Property(t => t.Image)
            .HasColumnName("Image")
            .HasColumnType("varchar(300)")
            .HasColumnOrder(3);
    }
}