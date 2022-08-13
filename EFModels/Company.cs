using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Mail { get; set; } = default!;

    public string Country { get; set; } = default!;

    public string? Address { get; set; }

    public List<Product> Products { get; set; } = default!;
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