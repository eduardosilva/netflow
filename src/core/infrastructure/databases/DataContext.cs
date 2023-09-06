using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

using Netflow.Entities;
using Netflow.Infrastructure.Databases.Configurations;

namespace Netflow.Infrastructure.Databases;

/// <summary>
/// Represents the main database context for your application.
/// </summary>
public class DataContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet of workflows in the database.
    /// </summary>
    /// <remarks>
    /// This property represents the collection of workflows stored in the database.
    /// It allows querying and manipulating country data using LINQ and Entity Framework Core.
    /// </remarks>
    public DbSet<Workflow> Workflows { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

    /// <summary>
    /// Gets or sets the DbSet of workflow instances in the database.
    /// </summary>
    /// <remarks>
    /// This property represents the collection of workflow instances stored in the database.
    /// It allows querying and manipulating country data using LINQ and Entity Framework Core.
    /// </remarks>
    public DbSet<WorkflowInstance> WorkflowsInstances { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used for configuring the context.</param>
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    { }

    /// <summary>
    /// This method is called when the model for the database is being created.
    /// It is used to configure the model using the Fluent API or by convention-based configuration.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for the database.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurations
        var assembly = typeof(DataContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(_ => typeof(IAuditable).IsAssignableFrom(_.ClrType)))
        {
            entityType.GetProperty("CreatedAt")
                      .SetDefaultValueSql("CURRENT_TIMESTAMP");

            entityType.GetProperty("CreatedBy")
                      .SetDefaultValue(Seed.NETFLOW_USER);

            entityType.GetProperty("CreatedBy").SetMaxLength(UserConfiguration.EMAIL_SIZE);
            entityType.GetProperty("LastModifiedBy").SetMaxLength(UserConfiguration.EMAIL_SIZE);
        }
    }
}

