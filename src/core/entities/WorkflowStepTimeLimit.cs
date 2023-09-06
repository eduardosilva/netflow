using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents the time limit configuration for a workflow step.
/// </summary>
public class WorkflowStepTimeLimit
{
    /// <summary>
    /// Gets or sets the date and time when the time limit expires.
    /// </summary>
    public DateTime ExpiresIn { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a value indicating whether automatic approval occurs when the time limit is reached.
    /// </summary>
    public bool AutoApproveOnThreshold { [DebuggerStepThrough] get; [DebuggerStepThrough] set; } = true;
}