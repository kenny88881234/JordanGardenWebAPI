using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Company
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    [Required]
    [MaxLength(20)]
    public string Name { get; set; } = default!;

    [JsonPropertyName("mail")]
    [Required]
    [MaxLength(50)]
    public string Mail { get; set; } = default!;

    [JsonPropertyName("country")]
    [Required]
    [MaxLength(20)]
    public string Country { get; set; } = default!;

    [JsonPropertyName("address")]
    [MaxLength(200)]
    public string? Address { get; set; }

    [JsonIgnore]
    public List<Product>? Products { get; set; }
}

public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .Property(c => c.Id)
            .IsRequired()
            .HasColumnOrder(0);

        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(20)")
            .HasColumnOrder(1);

        builder
            .Property(c => c.Mail)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasColumnOrder(2);

        builder
            .Property(c => c.Country)
            .IsRequired()
            .HasColumnType("varchar(20)")
            .HasColumnOrder(3);

        builder
            .Property(c => c.Address)
            .HasColumnType("varchar(200)")
            .HasColumnOrder(4);
    }
}