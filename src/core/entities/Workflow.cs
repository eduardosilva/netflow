using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents a workflow entity that manages a series of steps and their instances.
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class Workflow : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Workflow"/> class.
    /// </summary>
    public Workflow()
    {
        Steps = new List<WorkflowStep>();
        WorkflowInstances = new List<WorkflowInstance>();
    }

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the description of the workflow.
    /// </summary>
    public string? Description { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a collection of steps in the workflow.
    /// </summary>
    public ICollection<WorkflowStep> Steps { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a collection of instances of this workflow.
    /// </summary>
    public ICollection<WorkflowInstance> WorkflowInstances { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Creates a new instance of the workflow and initializes its steps.
    /// </summary>
    /// <returns>A newly created <see cref="WorkflowInstance"/>.</returns>
    public WorkflowInstance CreateNewInstance()
    {
        if (!Steps.Any())
            throw new WorkflowHasNoStepsException();

        if (Steps.All(s => s.RequiredApprovals.Any()) == false)
            throw new WorkflowNoStepsWithRequiredApprovalsException();

        var newInstance = new WorkflowInstance(this);
        WorkflowInstances.Add(newInstance);

        var orderedSteps = Steps.OrderBy(step => step.Order);

        foreach (var step in orderedSteps)
        {
            var stepInstance = new WorkflowStepInstance(newInstance, step);
            newInstance.Steps.Add(stepInstance);
        }

        return newInstance;
    }
}