using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Project.Models;

namespace Project.Data.Configuration;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.OrderStatus)
            .HasConversion<string>();

        builder.Property(o => o.PaymentMethod)
            .HasConversion<string>();

        builder.Property(o => o.ShippingAddress)
            .HasMaxLength(200);

        builder.Property(o => o.ShippingMethod)
            .HasConversion<string>();

        builder.Property(o => o.TrackingNumber)
            .HasMaxLength(50);
    }
}