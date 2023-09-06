
using System.Diagnostics;
namespace Netflow.Entities;

/// <summary>
/// Interface for auditable entities, which includes properties for creation and modification tracking.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedAt { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the user who created the entity.
    /// </summary>
    string CreatedBy { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the date and time of the last modification to the entity.
    /// </summary>
    DateTime? LastModifiedAt { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the user who made the last modification to the entity.
    /// </summary>
    string? LastModifiedBy { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}