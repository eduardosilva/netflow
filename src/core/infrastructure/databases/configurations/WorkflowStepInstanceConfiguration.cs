
using Netflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflow.Infrastructure.Databases.Configurations;

public class WorkflowStepInstanceConfiguration : IEntityTypeConfiguration<WorkflowStepInstance>
{
    public void Configure(EntityTypeBuilder<WorkflowStepInstance> builder)
    {
        // Table
        builder.ToTable("workflow_step_instances");

        // Keys
        builder.HasKey(_ => _.Id);

        // Properties

        // Relations
        builder.OwnsOne(_ => _.WorkflowStepTimeLimit, stepConfiguration =>
        {
            stepConfiguration.Property(_ => _.ExpiresIn).HasColumnName("expires_in");
            stepConfiguration.Property(_ => _.AutoApproveOnThreshold).HasColumnName("auto_approve_on_threshold");
        });

        // Indexes

        //Seed
    }
}

