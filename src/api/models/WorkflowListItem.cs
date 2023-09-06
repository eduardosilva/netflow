using System.Diagnostics;

namespace Netflow.Models;

/// <summary>
/// Represents a list item for a workflow.
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class WorkflowListItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow.
    /// </summary>
    /// <example>1</example>
    public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    /// <example>Sample Workflow</example>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}