using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EcoStore.DAL.Entities;

namespace EcoStore.DAL.EF.Configuration;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasColumnType("money");

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(2048);

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}