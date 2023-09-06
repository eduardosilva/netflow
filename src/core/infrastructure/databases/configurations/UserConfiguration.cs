using Netflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflow.Infrastructure.Databases.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public static ushort EMAIL_SIZE = 350;
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table
        builder.ToTable("users");

        // Keys
        builder.HasKey(_ => _.Id);

        // Properties
        builder.Property(_ => _.Email)
               .HasMaxLength(EMAIL_SIZE)
               .IsRequired()
               .IsUnicode(false);
        // Relations

        // Indexes
        builder.HasIndex(_ => _.Email).IsUnique();

        //Seed
        builder.HasData(Seed.User);
    }
}