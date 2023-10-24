using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Data.Configuration;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}