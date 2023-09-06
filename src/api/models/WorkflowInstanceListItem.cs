using System.Diagnostics;

namespace Netflow.Models;

/// <summary>
/// Represents a list item for a workflow instance.
/// </summary>
///
[DebuggerDisplay("{Name,nq}")]
public class WorkflowInstanceListItem
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow instance.
    /// </summary>
    /// <example>101</example>
    public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the name of the workflow instance.
    /// </summary>
    /// <example>Sample Workflow Instance</example>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a value indicating whether the workflow instance is completed.
    /// </summary>
    /// <example>false</example>
    public bool IsCompleted { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the name of the current step within the workflow instance.
    /// </summary>
    /// <example>Step 2</example>
    public string CurrentStep { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the date and time when the workflow instance was created.
    /// </summary>
    /// <example>2023-09-14T10:30:00Z</example>
    public DateTime CreatedAt { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}
