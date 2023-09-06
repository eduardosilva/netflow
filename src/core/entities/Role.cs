using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents a role within the workflow, defining its name and description.
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class Role : Entity
{
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    public string? Description { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}