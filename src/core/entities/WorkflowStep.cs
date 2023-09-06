using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents a step within a workflow, defining its properties and relationships.
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class WorkflowStep : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowStep"/> class.
    /// </summary>
    public WorkflowStep()
    {
        RequiredApprovals = new List<Role>();

    }

    /// <summary>
    /// Gets or sets the name of the workflow step.
    /// </summary>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the description of the workflow step.
    /// </summary>
    public string? Description { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the next step to follow when this step is approved.
    /// </summary>
    public WorkflowStep? ApprovedNextStep { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the next step to follow when this step is rejected.
    /// </summary>
    public WorkflowStep? RejectedNextStep { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a collection of roles that are required to approve this step.
    /// </summary>
    public ICollection<Role> RequiredApprovals { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the order or sequence in which this step appears within the workflow.
    /// </summary>
    public int? Order { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the configuration for time limits associated with workflow steps.
    /// </summary>
    /// <remarks>
    /// This property allows you to specify time limits and related settings for workflow steps.
    /// </remarks>
    /// <value>
    /// A <see cref="WorkflowStepTimeLimitConfiguration"/> object representing the configuration.
    /// If not set, time limits are not enforced for workflow steps.
    /// </value>
    /// <seealso cref="WorkflowStepTimeLimitConfiguration"/>
    public WorkflowStepTimeLimitConfiguration? StepTimeLimitConfiguration { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}
