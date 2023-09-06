namespace Netflow.Entities;

public class WorkflowCreationException : Exception
{
    public WorkflowCreationException(string message, Exception ex) : base(message, ex) { }
}