
using System.Diagnostics;

namespace Netflow.Entities;

/// <summary>
/// Represents an entity with a specified type for its unique identifier.
/// </summary>
/// <typeparam name="T">The type of the unique identifier.</typeparam>
public class Entity<T> : IAuditable
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public T Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the user who created the entity.
    /// </summary>
    public string CreatedBy { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the date and time of the last modification to the entity.
    /// </summary>
    public DateTime? LastModifiedAt { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the user who made the last modification to the entity.
    /// </summary>
    public string? LastModifiedBy { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Determines whether the current entity is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns>
    /// <c>true</c> if the current entity is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Two entities are considered equal if they are of the same type and have the same ID.
    /// </remarks>
    /// <seealso cref="GetHashCode"/>
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Entity<T>)obj;

        // Compare the ID property for equality.
        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// Serves as a hash function for the current entity.
    /// </summary>
    /// <returns>
    /// A hash code for the current entity.
    /// </returns>
    /// <remarks>
    /// The hash code is based on the entity's ID property.
    /// </remarks>
    /// <seealso cref="Equals(object)"/>
    public override int GetHashCode()
    {
        // Use the hash code of the ID property for hashing.
        return EqualityComparer<T>.Default.GetHashCode(Id);
    }
}

/// <summary>
/// Represents a non-generic entity with an integer unique identifier.
/// </summary>
public class Entity : Entity<int>
{
}
