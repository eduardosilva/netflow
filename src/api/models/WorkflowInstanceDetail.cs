using System.Diagnostics;

namespace Netflow.Models;

/// <summary>
/// Represents a detailed view of a workflow instance.
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class WorkflowInstanceDetail
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow instance.
    /// </summary>
    /// <example>1</example>
    public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the name of the workflow instance.
    /// </summary>
    /// <example>Sample Workflow Instance</example>
    public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a value indicating whether the workflow instance is completed.
    /// </summary>
    /// <example>false</example>
    public bool IsCompleted { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the current step instance within the workflow.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// {
    ///     "Id": 101,
    ///     "Name": "Step 1",
    ///     "IsApproved": false,
    ///     "StepTimeLimit": {
    ///         "ExpiresIn": "2023-09-30T12:00:00Z",
    ///         "AutoApproveOnThreshold": true
    ///     }
    /// }
    /// ]]>
    /// </example>
    public StepInstance CurrentStep { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets a collection of step instances within the workflow.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// [
    ///     {
    ///         "Id": 101,
    ///         "Name": "Step 1",
    ///         "IsApproved": false,
    ///         "StepTimeLimit": {
    ///             "ExpiresIn": "2023-09-30T12:00:00Z",
    ///             "AutoApproveOnThreshold": true
    ///         }
    ///     },
    ///     {
    ///         "Id": 102,
    ///         "Name": "Step 2",
    ///         "IsApproved": true,
    ///         "StepTimeLimit": null
    ///     }
    /// ]
    /// ]]>
    /// </example>
    public ICollection<StepInstance> Steps { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the workflow instance was created.
    /// </summary>
    /// <example>2023-09-14T10:30:00Z</example>
    public DateTime CreatedAt { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Represents a step instance within the workflow.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class StepInstance
    {
        /// <summary>
        /// Gets or sets the unique identifier of the step instance.
        /// </summary>
        /// <example>101</example>
        public int Id { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets the name of the step instance.
        /// </summary>
        /// <example>Step 1</example>
        public string Name { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets a value indicating whether the step instance is approved.
        /// </summary>
        /// <example>false</example>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the time limit configuration for the step instance.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// {
        ///     "ExpiresIn": "2023-09-30T12:00:00Z",
        ///     "AutoApproveOnThreshold": true
        /// }
        /// ]]>
        /// </example>
        public StepTimeLimit? StepTimeLimit { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
    }

    /// <summary>
    /// Represents the time limit configuration for a step instance.
    /// </summary>
    public class StepTimeLimit
    {
        /// <summary>
        /// Gets or sets the expiration date and time for the step instance.
        /// </summary>
        /// <example>2023-09-30T12:00:00Z</example>
        public DateTime ExpiresIn { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

        /// <summary>
        /// Gets or sets a value indicating whether the step should be auto-approved when the time threshold is reached.
        /// </summary>
        /// <example>true</example>
        public bool AutoApproveOnThreshold { [DebuggerStepThrough] get; [DebuggerStepThrough] set; } = true;
    }
}