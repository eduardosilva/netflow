using Netflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflow.Infrastructure.Databases.Configurations;

public class WorkflowInstanceConfiguration : IEntityTypeConfiguration<WorkflowInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowInstance> builder)
    {
        // Table
        builder.ToTable("workflow_instances");

        // Keys
        builder.HasKey(_ => _.Id);

        // Properties

        // Relations
        builder.HasMany(_ => _.Steps)
            .WithOne(_ => _.WorkflowInstance);

        // Indexes

        //Seed
    }
}