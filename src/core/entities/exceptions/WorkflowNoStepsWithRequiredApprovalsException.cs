namespace Netflow.Entities;

public class WorkflowNoStepsWithRequiredApprovalsException : Exception
{
    public WorkflowNoStepsWithRequiredApprovalsException()
        : base("Workflow must have at least one step with required approvals to initiate.")
    {
    }
}