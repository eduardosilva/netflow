using Netflow.Entities;

namespace Netflow.Services;

/// <summary>
/// Interface for a service that creates a new instance of a <see cref="Workflow"/>.
/// </summary>
public interface ICreateNewWorkflowInstanceService
{

    /// <summary>
    /// Creates a new instance of a <see cref="Workflow"/>.
    /// </summary>
    /// <param name="workflow">The workflow for which to create an instance.</param>
    /// <returns>The newly created <see cref="WorkflowInstance"/> or null if the operation fails.</returns>
    Task<WorkflowInstance?> CreateAsync(Workflow workflow);
}