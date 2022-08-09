using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Order
{
    public int Id { get; set; }

    public decimal? ShipPrice { get; set; }

    public decimal? OtherPrice { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime Uptimestamp { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = default!;
}

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .Property(o => o.Id)
            .IsRequired()
            .HasColumnOrder(0);

        builder
            .Property(o => o.ShipPrice)
            .HasColumnName("Ship_Price")
            .HasColumnType("decimal(7,3)")
            .HasColumnOrder(1);

        builder
            .Property(o => o.OtherPrice)
            .HasColumnName("Other_Price")
            .HasColumnType("decimal(7,3)")
            .HasColumnOrder(2);

        builder
            .Property(o => o.CreateTime)
            .IsRequired()
            .HasColumnName("Create_Time")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .HasColumnOrder(3);

        builder
            .Property(o => o.Uptimestamp)
            .IsRequired()
            .HasColumnName("Update_Time")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .HasColumnOrder(4);
    }
}