using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Project.Models;

namespace Project.Data.Configuration;

public class OrderedProductEntityTypeConfiguration : IEntityTypeConfiguration<OrderedProduct>
{
    public void Configure(EntityTypeBuilder<OrderedProduct> builder)
    {
        builder.HasKey(op => new { op.ProductId, op.OrderId });

        builder.Property(op => op.ProductPrice)
            .HasColumnType("money");
    }
}