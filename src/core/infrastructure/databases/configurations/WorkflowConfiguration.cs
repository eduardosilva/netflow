using Netflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflow.Infrastructure.Databases.Configurations;

public class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        // Table
        builder.ToTable("workflows");

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
        builder.HasMany(_ => _.Steps)
            .WithOne();

        // Indexes
        builder.HasIndex(_ => _.Name).IsUnique();

        //Seed
        builder.HasData(Seed.PayrollProcess);
    }
}