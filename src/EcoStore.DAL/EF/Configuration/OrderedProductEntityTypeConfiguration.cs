using EcoStore.DAL.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoStore.DAL.EF.Configuration;

public class OrderedProductEntityTypeConfiguration : IEntityTypeConfiguration<OrderedProduct>
{
    public void Configure(EntityTypeBuilder<OrderedProduct> builder)
    {
        builder.HasKey(op => new { op.ProductId, op.OrderId });

        builder.Property(op => op.ProductPrice)
            .HasColumnType("money");
    }
}