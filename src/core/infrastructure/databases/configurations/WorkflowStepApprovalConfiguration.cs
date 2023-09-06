using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netflow.Entities;

namespace Netflow.Infrastructure.Databases.Configurations;

public class WorkflowStepApprovalConfiguration : IEntityTypeConfiguration<WorkflowStepApproval>
{
    public void Configure(EntityTypeBuilder<WorkflowStepApproval> builder)
    {
        // Table
        builder.ToTable("workflow_step_approvals");

        // Keys
        builder.HasKey(_ => _.Id);

        // Properties
        builder.Property(_ => _.Comments)
               .HasMaxLength(500)
               .IsUnicode(false);

        // Relations

        // Indexes

        //Seed
    }
}