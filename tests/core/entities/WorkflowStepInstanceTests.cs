namespace Netflow.Entities.Tests;
public class WorkflowStepInstanceTests
{

    [Fact]
    public void Constructor_WithoutStepTimeLimitConfiguration_DoesNotSetWorkflowStepTimeLimit()
    {
        // Arrange
        var workflow = new Workflow();
        var instance = new WorkflowInstance(workflow);
        var step = new WorkflowStep(); // No StepTimeLimitConfiguration

        // Act
        var stepInstance = new WorkflowStepInstance(instance, step);

        // Assert
        Assert.Null(stepInstance.WorkflowStepTimeLimit);
    }
}