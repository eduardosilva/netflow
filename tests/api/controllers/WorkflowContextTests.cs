using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflow.Entities;
using Netflow.Infrastructure.Databases;
using Netflow.Models;
using Netflow.Profiles;

namespace Netflow.Controllers.Tests;

public class WorkflowControllerTests : IDisposable
{
    private readonly DbContextOptions<DataContext> dbContextOptions;
    private readonly DataContext dbContext;
    private readonly IMapper mapper;

    public WorkflowControllerTests()
    {
        // Set up the common test context in the constructor
        dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        dbContext = new DataContext(dbContextOptions);

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<WorkflowProfile>();
        });
        mapper = new Mapper(mapperConfiguration);

        // Seed some sample data
        dbContext.Workflows.Add(new Workflow
        {
            Id = 1,
            Name = "Workflow 1",
            Steps = new List<WorkflowStep>(new[]
                { new WorkflowStep { Id = 1, Name = "Step 1", CreatedAt = DateTime.UtcNow, CreatedBy = "netflow" } }),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "netflow"
        });
        dbContext.Workflows.Add(new Workflow { Id = 2, Name = "Workflow 2", CreatedAt = DateTime.UtcNow, CreatedBy = "netflow" });
        dbContext.SaveChanges();
    }

    public void Dispose()
    {
        // Clean up resources after each test
        dbContext.Dispose();
    }

    [Fact]
    public async Task GetWorkflows_ReturnsWorkflowList()
    {
        // Arrange
        var controller = new WorkflowController(dbContext, mapper);

        // Act
        var result = await controller.GetWorkflowsAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<WorkflowListItem[]>(okResult.Value);

        Assert.Equal(2, model.Count());
        Assert.Equal("Workflow 1", model.ElementAt(0).Name);
        Assert.Equal("Workflow 2", model.ElementAt(1).Name);
    }

    [Fact]
    public async Task GetWorkflow_ReturnsWorkflowDetail()
    {
        // Arrange
        var controller = new WorkflowController(dbContext, mapper);

        // Act
        var result = await controller.GetWorkflowAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<WorkflowDetail>(okResult.Value);

        Assert.Equal("Workflow 1", model.Name);
        Assert.Single(model.Steps);
        Assert.Equal("Step 1", model.Steps.ElementAt(0).Name);
    }

    [Fact]
    public async Task GetWorkflow_NotFound()
    {
        // Arrange
        var controller = new WorkflowController(dbContext, mapper);

        // Act
        var result = await controller.GetWorkflowAsync(10);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

}