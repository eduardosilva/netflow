using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents a user.
/// </summary>
[DebuggerDisplay("{Email,nq}")]
public class User
{
    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    public User()
    {
    }
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
    /// <summary>
    /// Gets or sets the Email of the user.
    /// </summary>
    public string Email { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
}
