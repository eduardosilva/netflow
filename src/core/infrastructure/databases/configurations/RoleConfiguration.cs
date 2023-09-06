using Netflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflow.Infrastructure.Databases.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Table
        builder.ToTable("roles");

        // Keys
        builder.HasKey(_ => _.Id);

        // Properties
        builder.Property(_ => _.Name)
               .HasMaxLength(150)
               .IsRequired()
               .IsUnicode(false);

        builder.Property(_ => _.Description)
               .HasMaxLength(500)
               .IsUnicode(false);

        // Relations

        // Indexes
        builder.HasIndex(_ => _.Name).IsUnique();

        //Seed
        builder.HasData(Seed.Roles);
    }
}