using System.Diagnostics;
using System.Linq.Expressions;

namespace Netflow.Entities;

/// <summary>
/// Represents an instance of a workflow, tracking its progress and steps.
/// </summary>
public class WorkflowInstance : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowInstance"/> class (protected constructor for entity framework).
    /// </summary>
    protected WorkflowInstance()
    {
        Steps = new List<WorkflowStepInstance>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowInstance"/> class with the specified base workflow.
    /// </summary>
    /// <param name="workflow">The base workflow associated with this instance.</param>
    public WorkflowInstance(Workflow workflow)
        : this()
    {
        BaseWorkflow = workflow;
        StartDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets or sets the current step within this workflow instance.
    /// </summary>
    public WorkflowStepInstance? CurrentStep { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets the base workflow associated with this instance.
    /// </summary>
    public Workflow BaseWorkflow { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets a value indicating whether this workflow instance has been completed.
    /// </summary>
    public bool IsCompleted { [DebuggerStepThrough] get; [DebuggerStepThrough] private set; }

    /// <summary>
    /// Gets the date and time when this workflow instance started.
    /// </summary>
    public DateTime StartDate { [DebuggerStepThrough] get; [DebuggerStepThrough] private set; }

    /// <summary>
    /// Gets the date and time when this workflow instance ended (if completed).
    /// </summary>
    public DateTime? EndDate { [DebuggerStepThrough] get; [DebuggerStepThrough] private set; }

    /// <summary>
    /// Gets or sets a collection of step instances within this workflow instance.
    /// </summary>
    public ICollection<WorkflowStepInstance> Steps { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Approves the current step within this workflow instance.
    /// </summary>
    /// <param name="username">The username of the approver.</param>
    /// <param name="comments">Comments provided by the approver.</param>
    public void ApproveStep(string username, string comments) => ChangeStepStatus(username, comments, isApproved: true);

    /// <summary>
    /// Rejects the current step within this workflow instance.
    /// </summary>
    /// <param name="username">The username of the rejector.</param>
    /// <param name="comments">Comments provided by the rejector.</param>
    public void RejectStep(string username, string comments) => ChangeStepStatus(username, comments, isApproved: false);

    private void ChangeStepStatus(string username, string comments, bool isApproved)
    {
        if (CurrentStep is null)
            throw new InvalidOperationException("Cannot change step status because the current step is null.");

        var approval = new WorkflowStepApproval();
        CurrentStep.Approvals.Add(approval);

        approval.IsApproved = isApproved;
        approval.Comments = comments;

        CurrentStep.IsApproved = CurrentStep.Approvals.All(_ => _.IsApproved == true);

        var nextStep = isApproved ? CurrentStep.BaseStep.ApprovedNextStep : CurrentStep.BaseStep.RejectedNextStep;
        if (nextStep == null)
        {
            CompleteWorkflow();
            return;
        }

        CurrentStep = Steps.FirstOrDefault(_ => _.BaseStep == nextStep);
        if (CurrentStep.BaseStep.StepTimeLimitConfiguration != null)
        {
            CurrentStep.WorkflowStepTimeLimit = new WorkflowStepTimeLimit
            {
                ExpiresIn = DateTime.UtcNow.AddMinutes(CurrentStep.BaseStep.StepTimeLimitConfiguration.MaximumTimeInMinutes),
                AutoApproveOnThreshold = CurrentStep.BaseStep.StepTimeLimitConfiguration.AutoApproveOnThreshold
            };
        }
    }

    private void CompleteWorkflow()
    {
        IsCompleted = true;
        EndDate = DateTime.UtcNow;
    }
    public override string ToString()
    {
        if (BaseWorkflow == null)
            return Id.ToString();

        return $"{BaseWorkflow.Name} ({Id})";
    }

    /// <summary>
    /// Generates an expression to filter <see cref="WorkflowInstance"/> entities with steps that have expired before a given date.
    /// </summary>
    /// <param name="date">The date to compare against.</param>
    /// <returns>An expression representing the filter condition.</returns>
    public static Expression<Func<WorkflowInstance, bool>> ExpiredStepBeforePredicate(DateTime date)
    {
        return _ => !_.IsCompleted
            && _.CurrentStep.IsApproved == null
            && _.CurrentStep.WorkflowStepTimeLimit != null
            && _.CurrentStep.WorkflowStepTimeLimit.ExpiresIn <= date;
    }
    /// <summary>
    /// Generates an expression to filter <see cref="WorkflowInstance"/> entities with steps that are set to expire after a given date.
    /// </summary>
    /// <param name="date">The date to compare against.</param>
    /// <returns>An expression representing the filter condition.</returns>
    public static Expression<Func<WorkflowInstance, bool>> ToExpireStepAfterPredicate(DateTime date)
    {
        return _ => !_.IsCompleted
            && _.CurrentStep.IsApproved == null
            && _.CurrentStep.WorkflowStepTimeLimit != null
            && _.CurrentStep.WorkflowStepTimeLimit.ExpiresIn >= date;
    }
}