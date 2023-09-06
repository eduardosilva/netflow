using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflow.Entities;
using Netflow.Infrastructure.Databases;
using Netflow.Models;
using Netflow.Services;

namespace Netflow.Controllers;

/// <summary>
/// Controller for managing workflow-related operations in the API.
/// </summary>
[ApiController]
[Route("api/workflows")]
public class WorkflowController : AppBaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowController"/> class.
    /// </summary>
    /// <param name="dataContext">The <see cref="DataContext"/> for data access.</param>
    /// <param name="mapper">The <see cref="IMapper"/> for object mapping.</param>
    public WorkflowController(DataContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {

    }

    /// <summary>
    /// Retrieves a list of workflows asynchronously.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing a list of <see cref="WorkflowListItem"/> if successful.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WorkflowListItem>))]
    public async Task<IActionResult> GetWorkflowsAsync()
    {
        var query = DataContext.Workflows;
        var result = await Mapper.ProjectTo<WorkflowListItem>(query, null).ToArrayAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a workflow by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the workflow to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="WorkflowDetail"/> if found, or a 404 Not Found response.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkflowDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWorkflowAsync(int id)
    {
        var query = DataContext.Workflows.Where(_ => _.Id == id)
                                         .Include(_ => _.Steps)
                                         .ThenInclude(_ => _.RequiredApprovals);
        var result = await Mapper.ProjectTo<WorkflowDetail>(query, null).FirstOrDefaultAsync();

        if (result == null) return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Creates a new instance of a workflow asynchronously.
    /// </summary>
    /// <param name="service">The <see cref="ICreateNewWorkflowInstanceService"/> used to create a new instance.</param>
    /// <param name="id">The ID of the workflow to create an instance for.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="WorkflowInstanceDetail"/> if successful, or a 404 Not Found response.</returns>
    [HttpPost("{id}/create-new-instance")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkflowInstanceDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateNewInstanceAsync([FromServices] ICreateNewWorkflowInstanceService service, int id)
    {
        var workflow = await DataContext.Workflows.Where(_ => _.Id == id)
                                                  .Include(_ => _.Steps)
                                                  .ThenInclude(_ => _.RequiredApprovals)
                                                  .FirstOrDefaultAsync();

        if (workflow == null) return NotFound();

        var instance = await service.CreateAsync(workflow);
        var result = Mapper.Map<WorkflowInstance, WorkflowInstanceDetail>(instance);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a list of instances for a workflow by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the workflow for which to retrieve instances.</param>
    /// <returns>An <see cref="IActionResult"/> containing a list of <see cref="WorkflowInstanceListItem"/> if successful.</returns>
    [HttpGet("{id}/instances")]
    public async Task<IActionResult> GetInstancesAsync(int id)
    {
        var query = DataContext.WorkflowsInstances.Where(_ => _.BaseWorkflow.Id == id);

        var result = await Mapper.ProjectTo<WorkflowInstanceListItem>(query, null).ToArrayAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves an instance of a workflow by ID and instance ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the workflow for which to retrieve the instance.</param>
    /// <param name="instanceId">The ID of the instance to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="WorkflowInstanceDetail"/> if found.</returns>
    [HttpGet("{id}/instances/{instanceId}")]
    public async Task<IActionResult> GetInstanceAsync(int id, int instanceId)
    {
        var query = DataContext.WorkflowsInstances.Where(_ => _.Id == instanceId && _.BaseWorkflow.Id == id);

        var result = await Mapper.ProjectTo<WorkflowInstanceDetail>(query, null).FirstOrDefaultAsync();
        return Ok(result);
    }

    /// <summary>
    /// Approves a workflow instance asynchronously.
    /// </summary>
    /// <param name="id">The ID of the workflow for which to approve the instance.</param>
    /// <param name="instanceId">The ID of the instance to approve.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="WorkflowInstanceDetail"/> if successful, or a 404 Not Found response.</returns>
    [HttpPost("{id}/instances/{instanceId}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkflowInstanceDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveAsync(int id, int instanceId) => await ChangeStatusAsync(id, instanceId, _ => _.ApproveStep("", ""));

    /// <summary>
    /// Rejects a workflow instance asynchronously.
    /// </summary>
    /// <param name="id">The ID of the workflow for which to reject the instance.</param>
    /// <param name="instanceId">The ID of the instance to reject.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="WorkflowInstanceDetail"/> if successful, or a 404 Not Found response.</returns>
    [HttpPost("{id}/instances/{instanceId}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkflowInstanceDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectAsync(int id, int instanceId) => await ChangeStatusAsync(id, instanceId, _ => _.RejectStep("", ""));

    /// <summary>
    /// Changes the status of a workflow instance asynchronously.
    /// </summary>
    /// <param name="id">The ID of the workflow for which to change the instance status.</param>
    /// <param name="instanceId">The ID of the instance to change status for.</param>
    /// <param name="changeStatus">An action to change the status of the instance.</param>
    /// <returns>An <see cref="IActionResult"/> containing a <see cref="WorkflowInstanceDetail"/> if successful, or a 404 Not Found response.</returns>
    private async Task<IActionResult> ChangeStatusAsync(int id, int instanceId, Action<WorkflowInstance> changeStatus)
    {
        var instance = await DataContext.WorkflowsInstances.Where(_ => _.Id == instanceId && _.BaseWorkflow.Id == id && _.IsCompleted == false)
                                                           .Include(_ => _.BaseWorkflow)
                                                           .Include(_ => _.CurrentStep.BaseStep.ApprovedNextStep)
                                                           .Include(_ => _.CurrentStep.BaseStep.ApprovedNextStep.StepTimeLimitConfiguration)
                                                           .Include(_ => _.CurrentStep.BaseStep.RejectedNextStep)
                                                           .Include(_ => _.CurrentStep.BaseStep.RejectedNextStep.StepTimeLimitConfiguration)
                                                           .Include(_ => _.Steps)
                                                           .FirstOrDefaultAsync();

        if (instance == null) return NotFound();

        changeStatus(instance);

        await DataContext.SaveChangesAsync();

        var result = Mapper.Map<WorkflowInstance, WorkflowInstanceDetail>(instance);
        return Ok(result);
    }

}
