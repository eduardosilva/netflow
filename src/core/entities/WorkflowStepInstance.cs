using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents an instance of a workflow step within a workflow instance, including its associated step and approvals.
/// </summary>
public class WorkflowStepInstance : Entity
{

    protected WorkflowStepInstance()
    {
        Approvals = new List<WorkflowStepApproval>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowStepInstance"/> class with the specified workflow instance and step.
    /// </summary>
    /// <param name="instance">The workflow instance to which this step instance belongs.</param>
    /// <param name="step">The base workflow step associated with this step instance.</param>
    public WorkflowStepInstance(WorkflowInstance instance, WorkflowStep step)
        : this()
    {
        WorkflowInstance = instance;
        BaseStep = step;

        // if (step.StepTimeLimitConfiguration != null)
        // {
        //     WorkflowStepTimeLimit = new WorkflowStepTimeLimit
        //     {
        //         ExpiresIn = DateTime.UtcNow.AddMinutes(step.StepTimeLimitConfiguration.MaximumTimeInMinutes),
        //         AutoApproveOnThreshold = step.StepTimeLimitConfiguration.AutoApproveOnThreshold
        //     };
        // }
    }

    /// <summary>
    /// Gets or sets the base workflow step associated with this step instance.
    /// </summary>
    public WorkflowStep BaseStep { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the base workflow instance associated with this step instance.
    /// </summary>
    public WorkflowInstance WorkflowInstance { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a collection of approvals associated with this step instance.
    /// </summary>
    public ICollection<WorkflowStepApproval> Approvals { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets the time limit configuration for the workflow step.
    /// </summary>
    public WorkflowStepTimeLimit? WorkflowStepTimeLimit { [DebuggerStepThrough] get; [DebuggerStepThrough] set;}

    /// <summary>
    /// Gets or sets a value indicating whether this step is approved.
    /// </summary>
    public bool? IsApproved { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    public override string ToString()
    {
        if (BaseStep == null)
        {
            return Id.ToString();
        }

        return $" { BaseStep.Name } ({Id})";
    }
}
