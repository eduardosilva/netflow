
namespace Netflow.Infrastructure.Databases;

public static class Seed
{
    public static string NETFLOW_USER = "netflow";
    public static readonly dynamic User = new { Id = 1, Email = "eduardo.lour.silva@gmail.com" };
    public static readonly dynamic AuditInfo = new
    {
        CreatedAt = new DateTime(2023, 9, 7).ToUniversalTime(),
        CreatedBy = NETFLOW_USER,
    };

    public static readonly List<dynamic> Roles = new List<dynamic>(new[]
    {
        new { Id = 1, Name = "Employee Supervisor", Description = "Supervisors review and approve the work hours and activities recorded by their respective team members." , AuditInfo.CreatedAt, AuditInfo.CreatedBy },
        new { Id = 2, Name = "Department Manager", Description = "Department review and approve the employee time records for their departments or teams. They ensure accuracy and compliance with department-specific policies." , AuditInfo.CreatedAt, AuditInfo.CreatedBy },
        new { Id = 3, Name = "Resource Manager", Description = "Resource managers review and approve the overall payroll data, ensuring that the allocation of resources and budget is accurate and aligns with the organization's goals." , AuditInfo.CreatedAt, AuditInfo.CreatedBy },
        new { Id = 4, Name = "Payroll Specialist", Description = "Payroll specialists are responsible for calculating gross pay, deductions, and net pay for each employee. They ensure accuracy and compliance with tax regulations." , AuditInfo.CreatedAt, AuditInfo.CreatedBy },
        new { Id = 5, Name = "Tax Compliance Officer", Description = "Tax compliance officers oversee tax withholding and ensure that it is done accurately based on employee information and tax laws." , AuditInfo.CreatedAt, AuditInfo.CreatedBy }
    });

    public static readonly dynamic PayrollProcess = new
    {
        Id = 1,
        Name = "Payroll Process",
        Description = "Manages employee compensation with precision, covering timekeeping, tax compliance, and benefit administration.",
        AuditInfo.CreatedAt,
        AuditInfo.CreatedBy
    };

    public static readonly List<dynamic> Steps = new List<dynamic>(new[]
    {
        new { Id = 5, Name = "Tax Withholding", Description = "Ensure accurate tax withholding based on employee information.", ApprovedNextStepId = (int?) null, AuditInfo.CreatedAt, AuditInfo.CreatedBy, WorkflowId = 1 },
        new { Id = 4, Name = "Payroll Calculation", Description = "Calculate gross pay, deductions, and net pay for each employee.", ApprovedNextStepId = (int?)5, AuditInfo.CreatedAt, AuditInfo.CreatedBy, WorkflowId = 1 },
        new { Id = 3, Name = "Resource Management Approval", Description = "Resource management department reviews and approves payroll data.", ApprovedNextStepId = (int?)4, AuditInfo.CreatedAt, AuditInfo.CreatedBy, WorkflowId = 1 },
        new { Id = 2, Name = "Manager Approval", Description = "Managers review and approve employee time records.", ApprovedNextStepId = (int?)3, AuditInfo.CreatedAt, AuditInfo.CreatedBy, WorkflowId = 1 },
        new { Id = 1, Name = "Employee Timekeeping", Description = "Employees record their work hours and activities.", ApprovedNextStepId = (int?)2, AuditInfo.CreatedAt, AuditInfo.CreatedBy, WorkflowId = 1 }
    });

    public static readonly List<dynamic> StepRoles = new List<dynamic>(new[]
    {
        new { WorkflowStepId = 1, RoleId = 1 },
        new { WorkflowStepId = 2, RoleId = 2 },
        new { WorkflowStepId = 3, RoleId = 3 },
        new { WorkflowStepId = 4, RoleId = 4 },
        new { WorkflowStepId = 5, RoleId = 5 },
    });

    public static readonly List<dynamic> StepLimitConfigurations = new List<dynamic>(new[]
    {
        new { WorkflowStepId = 5, MaximumTimeInMinutes = 5, AutoApproveOnThreshold = true }
    });
}