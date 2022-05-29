using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public Order Order { get; set; } = default!;

    public Product Product { get; set; } = default!;
}

public class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder
            .Property(od => od.Id)
            .IsRequired()
            .HasColumnOrder(0);

        builder
            .Property(od => od.OrderId)
            .IsRequired()
            .HasColumnName("Order_Id")
            .HasColumnType("int")
            .HasColumnOrder(1);

        builder
            .Property(od => od.ProductId)
            .IsRequired()
            .HasColumnName("Product_Id")
            .HasColumnType("int")
            .HasColumnOrder(2);

        builder
            .Property(od => od.Quantity)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnOrder(3);

        builder
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ForeignKey_OrderId");

        builder
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("ForeignKey_ProductId");
    }
}