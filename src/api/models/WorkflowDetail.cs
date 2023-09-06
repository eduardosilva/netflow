using System.Diagnostics;

namespace Netflow.Models;

/// <summary>
/// Represents a detailed view of a workflow.
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class WorkflowDetail
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow.
    /// </summary>
    /// <example>1</example>
    public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    /// <example>Sample Workflow</example>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the description of the workflow.
    /// </summary>
    /// <example>This is a sample workflow description.</example>
    public string Description { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a collection of workflow steps.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// [
    ///     {
    ///         "Id": 101,
    ///         "Name": "Step 1",
    ///         "RequiredApprovals": ["User1", "User2"],
    ///         "StepTimeLimitConfiguration": {
    ///             "MaximumTimeInMinutes": 30,
    ///             "AutoApproveOnThreshold": true
    ///         }
    ///     },
    ///     {
    ///         "Id": 102,
    ///         "Name": "Step 2",
    ///         "RequiredApprovals": ["User3", "User4"],
    ///         "StepTimeLimitConfiguration": {
    ///             "MaximumTimeInMinutes": 45,
    ///             "AutoApproveOnThreshold": false
    ///         }
    ///     }
    /// ]
    /// ]]>
    /// </example>
    public ICollection<Step> Steps { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Represents a workflow step.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class Step
    {
        /// <summary>
        /// Gets or sets the unique identifier of the step.
        /// </summary>
        /// <example>101</example>
        public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets the name of the step.
        /// </summary>
        /// <example>Step 1</example>
        public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets a collection of required approvals for the step.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// ["User1", "User2"]
        /// ]]>
        /// </example>
        public ICollection<string> RequiredApprovals { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets the time limit configuration for the step.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// {
        ///     "MaximumTimeInMinutes": 30,
        ///     "AutoApproveOnThreshold": true
        /// }
        /// ]]>
        /// </example>
        public StepTimeLimitConfiguration? StepTimeLimitConfiguration { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
    }

    /// <summary>
    /// Represents the time limit configuration for a workflow step.
    /// </summary>
    public class StepTimeLimitConfiguration
    {
        /// <summary>
        /// Gets or sets the maximum time allowed for the step in minutes.
        /// </summary>
        /// <example>30</example>
        public int MaximumTimeInMinutes { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets a value indicating whether the step should be auto-approved when the time threshold is reached.
        /// </summary>
        /// <example>true</example>
        public bool AutoApproveOnThreshold { [DebuggerStepThrough] get; [DebuggerStepThrough] set; } = true;
    }
}