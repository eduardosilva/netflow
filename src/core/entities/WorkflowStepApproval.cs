using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents an approval associated with a workflow step, tracking its approval status and comments.
/// </summary>
public class WorkflowStepApproval : Entity
{
    /// <summary>
    /// Gets or sets a value indicating whether the approval is approved (<c>true</c>), rejected (<c>false</c>), or pending (<c>null</c>).
    /// </summary>
    public bool? IsApproved { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets comments provided with the approval or rejection of a workflow step.
    /// </summary>
    public string Comments { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}