using NuGet.Frameworks;

namespace Netflow.Entities.Tests;
public class WorkflowInstanceTests
{
    [Fact]
    public void Constructor_WithWorkflow_SetsBaseWorkflowAndStartDate()
    {
        // Arrange
        var workflow = new Workflow();
        var expectedStartDate = DateTime.UtcNow;

        // Act
        var workflowInstance = new WorkflowInstance(workflow);

        // Assert
        Assert.Equal(workflow, workflowInstance.BaseWorkflow);
        Assert.Equal(expectedStartDate.Date, workflowInstance.StartDate.Date); // Compare dates for consistency
    }

    [Fact]
    public void ApproveStep_CurrentStepNotWaiting_ThrowsInvalidOperationException()
    {
        // Arrange
        var workflow = new Workflow();
        var workflowInstance = new WorkflowInstance(workflow);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => workflowInstance.ApproveStep("user", "comments"));
    }

    [Fact]
    public void RejectStep_CurrentStepNotWaiting_ThrowsInvalidOperationException()
    {
        // Arrange
        var workflow = new Workflow();
        var workflowInstance = new WorkflowInstance(workflow);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => workflowInstance.RejectStep("user", "comments"));
    }

    [Fact]
    public void ApproveStep_AllApprovalsGiven_CompletesWorkflow()
    {
        // Arrange
        var workflow = new Workflow();
        var workflowInstance = new WorkflowInstance(workflow);
        var role = new Role { Name = "Role A" };
        var workflowStep = new WorkflowStep() { RequiredApprovals = new[] { role } };
        var step = new WorkflowStepInstance(workflowInstance, workflowStep);

        workflowInstance.CurrentStep = step;

        // Act
        workflowInstance.ApproveStep("user", "comments");

        // Assert
        Assert.True(workflowInstance.IsCompleted);
        Assert.NotNull(workflowInstance.EndDate);
    }

    [Fact]
    public void ApproveStep_ValidStep_ChangesStepStatus()
    {
        // Arrange
        var workflow = new Workflow();
        var workflowInstance = new WorkflowInstance(workflow);
        var role = new Role { Name = "Role A" };
        var secondStep = new WorkflowStep() { Id = 2, Name = "Second Step", RequiredApprovals = new[] { role } };
        var firstStep = new WorkflowStep() { Id = 1, Name = "First Step", RequiredApprovals = new[] { role }, ApprovedNextStep = secondStep };

        workflowInstance.Steps.Add(new WorkflowStepInstance(workflowInstance, firstStep));
        workflowInstance.Steps.Add(new WorkflowStepInstance(workflowInstance, secondStep));
        var currentStep = workflowInstance.Steps.ElementAt(0);

        workflowInstance.CurrentStep = currentStep;

        // Act
        workflowInstance.ApproveStep("user", "comments");

        // Assert
        Assert.True(currentStep.Approvals.ElementAt(0).IsApproved);
        Assert.False(workflowInstance.IsCompleted);
        Assert.Null(workflowInstance.EndDate);
        Assert.Equal(secondStep, workflowInstance.CurrentStep.BaseStep);
    }
    [Fact]
    public void RejectStep_AllApprovalsGiven_CompletesWorkflow()
    {
        // Arrange
        var workflow = new Workflow();
        var workflowInstance = new WorkflowInstance(workflow);
        var role = new Role { Name = "Role A" };
        var workflowStep = new WorkflowStep() { RequiredApprovals = new[] { role } };
        var step = new WorkflowStepInstance(workflowInstance, workflowStep);

        workflowInstance.CurrentStep = step;

        // Act
        workflowInstance.RejectStep("user", "comments");

        // Assert
        Assert.True(workflowInstance.IsCompleted);
        Assert.NotNull(workflowInstance.EndDate);
    }

    [Fact]
    public void RejectStep_ValidStep_ChangesStepStatus()
    {
        // Arrange
        var workflow = new Workflow();
        var workflowInstance = new WorkflowInstance(workflow);
        var role = new Role { Name = "Role A" };
        var secondStep = new WorkflowStep() { Id = 2, Name = "Second Step", RequiredApprovals = new[] { role } };
        var firstStep = new WorkflowStep() { Id = 1, Name = "First Step", RequiredApprovals = new[] { role }, RejectedNextStep = secondStep };

        workflowInstance.Steps.Add(new WorkflowStepInstance(workflowInstance, firstStep));
        workflowInstance.Steps.Add(new WorkflowStepInstance(workflowInstance, secondStep));
        var currentStep = workflowInstance.Steps.ElementAt(0);

        workflowInstance.CurrentStep = currentStep;

        // Act
        workflowInstance.RejectStep("user", "comments");

        // Assert
        Assert.False(currentStep.Approvals.ElementAt(0).IsApproved);
        Assert.False(workflowInstance.IsCompleted);
        Assert.Null(workflowInstance.EndDate);
        Assert.Equal(secondStep, workflowInstance.CurrentStep.BaseStep);
    }
}