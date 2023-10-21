using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Project.Models;

namespace Project.Data.Configuration;

public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(b => b.Name)
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}