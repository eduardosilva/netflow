namespace Netflow.Entities.Tests;

public class WorkflowTests
{
    [Fact]
    public void CreateNewInstance_NoSteps_ThrowsWorkflowHasNoStepsException()
    {
        // Arrange
        var workflow = new Workflow();
        // Make sure there are no steps in the workflow.

        // Act & Assert
        Assert.Throws<WorkflowHasNoStepsException>(() => workflow.CreateNewInstance());
    }

    [Fact]
    public void CreateNewInstance_NoStepsWithRequiredApprovals_ThrowsWorkflowNoStepsWithRequiredApprovalsException()
    {
        // Arrange
        var role = new Role { Name = "Role A" };
        var workflow = new Workflow
        {
            Steps = new List<WorkflowStep>
            {
                new WorkflowStep { Name = "That is the seconds step", Order = 2, RequiredApprovals = new []{ role } },
                new WorkflowStep { Name = "That is the first step", Order = 1 },
            }
        };

        // Act & Assert
        Assert.Throws<WorkflowNoStepsWithRequiredApprovalsException>(() => workflow.CreateNewInstance());
    }

    [Fact]
    public void CreateNewInstance_WithSteps_CreatesNewInstanceAndCurrentStep()
    {
        // Arrange
        var role = new Role { Name = "Role A" };
        var workflow = new Workflow
        {
            Steps = new List<WorkflowStep>
            {
                new WorkflowStep { Name = "That is the seconds step", Order = 2, RequiredApprovals = new []{ role } },
                new WorkflowStep { Name = "That is the first step", Order = 1, RequiredApprovals = new [] { role } },
            }
        };

        // Act
        var newInstance = workflow.CreateNewInstance();

        // Assert
        Assert.NotNull(newInstance);
    }
}