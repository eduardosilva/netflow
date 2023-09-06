using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents the configuration for step time limits.
/// </summary>
public class WorkflowStepTimeLimitConfiguration
{
    /// <summary>
    /// Gets or sets the maximum time limit in minutes for a step.
    /// </summary>
    public int MaximumTimeInMinutes { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// [DebuggerStepThrough] gets or sets a value indicating whether auto-approval should occur when the time limit threshold is reached.
    /// </summary>
    /// <remarks>
    /// By default, <see cref="AutoApproveOnThreshold"/> is set to <c>true</c>.
    /// </remarks>
    public bool AutoApproveOnThreshold { [DebuggerStepThrough] get; [DebuggerStepThrough] set; } = true;
}
