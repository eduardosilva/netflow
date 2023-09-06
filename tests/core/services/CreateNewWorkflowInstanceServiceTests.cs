using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Netflow.Entities;
using Netflow.Infrastructure.Databases;

namespace Netflow.Services;

public class CreateNewWorkflowInstanceServiceTests : IDisposable
{
    private readonly string _connectionString;
    private readonly DbContextOptions<DataContext> _options;

    public CreateNewWorkflowInstanceServiceTests()
    {
        // Create a unique SQLite database file for each test run
        _connectionString = $"Data Source={Guid.NewGuid()}.db";

        _options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(_connectionString)
            .Options;

        using var dbContext = new DataContext(_options);
        dbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        // Clean up the SQLite database file after tests are done
        File.Delete(_connectionString);
    }

    [Fact]
    public async Task CreateAsync_ValidWorkflow_ReturnsWorkflowInstance()
    {
        // Arrange
        using var dbContext = new DataContext(_options);

        // Seed some sample data
        var workflow = new Workflow
        {
            Name = "Workflow 1",
            Steps = new List<WorkflowStep>(new[]
                {
                    new WorkflowStep
                    {
                        Name = "Step 1",
                        RequiredApprovals = new Role [] { new Role { Name = "Role 1", CreatedAt = DateTime.UtcNow, CreatedBy = "netflow" } },
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "netflow"
                    }
                }),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "netflow"
        };

        dbContext.Workflows.Add(workflow);
        dbContext.Workflows.Add(new Workflow { Name = "Workflow 2", CreatedAt = DateTime.UtcNow, CreatedBy = "netflow" });
        dbContext.SaveChanges();

        var service = new CreateNewWorkflowInstanceService(dbContext);

        // Act
        var result = await service.CreateAsync(workflow);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateAsync_NullWorkflow_ThrowsArgumentNullException()
    {
        // Arrange
        using var context = new DataContext(_options);
        var service = new CreateNewWorkflowInstanceService(context);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(null));
    }
}