namespace Netflow.Entities;
public class WorkflowHasNoStepsException : Exception
{
    public WorkflowHasNoStepsException()
        : base("Workflow cannot be started because it does not contain any steps. Please add at least one step to initiate the workflow.")
    {
    }
}