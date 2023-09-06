using Netflow.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Netflow.Infrastructure.Databases.Configurations;

public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
{
    public void Configure(EntityTypeBuilder<WorkflowStep> builder)
    {
        // Table
        builder.ToTable("workflow_steps");

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
        builder.HasOne(_ => _.ApprovedNextStep)
               .WithOne();

        builder.HasOne(_ => _.RejectedNextStep)
               .WithOne();

        builder.HasMany(_ => _.RequiredApprovals)
               .WithMany()
               .UsingEntity<RoleWorkflowStep>(
                      _ => _.HasOne<Role>().WithMany(),
                      _ => _.HasOne<WorkflowStep>().WithMany(),
                      _ =>
                      {
                          _.ToTable("workflow_step_roles");
                          _.HasKey(__ => new { __.RoleId, __.WorkflowStepId });
                          _.HasData(Seed.StepRoles);
                      });

        builder.OwnsOne(_ => _.StepTimeLimitConfiguration, stepConfiguration =>
        {
            stepConfiguration.Property(_ => _.MaximumTimeInMinutes).HasColumnName("maximum_time_in_minutes");
            stepConfiguration.Property(_ => _.AutoApproveOnThreshold).HasColumnName("auto_approve_on_threshold");
            stepConfiguration.HasData(Seed.StepLimitConfigurations);
        });

        // Indexes

        //Seed
        builder.HasData(Seed.Steps);
    }
}

public class RoleWorkflowStep
{
    public int RoleId { get; set; }
    public int WorkflowStepId { get; set; }
}
