using System;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Netflow.Jobs;

public class Program
{
    public class Options
    {
        [Option('j', "job", Required = true, HelpText = "Specify the job to run (e.g., WorkflowStepStatusChanger)")]
        public string JobName { get; set; }
    }

    public static async Task Main(string[] args)
    {
        // Create a service collection
        var services = new ServiceCollection();

        // Configure services using the Startup class
        var startup = new Startup();
        startup.ConfigureServices(services);

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(async options =>
            {
                // Create an instance of the selected job
                IJob job;
                switch (options.JobName)
                {
                    case "WorkflowStepStatusChanger":
                        job = serviceProvider.GetRequiredService<IWorkflowStepStatusChangerJob>();
                        break;
                    default:
                        Console.WriteLine($"Invalid job name: {options.JobName}");
                        return;
                }

                // Create a CancellationTokenSource
                using (var cts = new CancellationTokenSource())
                {
                    // Handle cancellation requests (e.g., Ctrl+C)
                    Console.CancelKeyPress += (sender, e) =>
                    {
                        e.Cancel = true; // Prevent the process from terminating immediately
                        cts.Cancel();    // Signal cancellation to the job
                    };

                    try
                    {
                        // Run the selected job with the cancellation token
                        await job.RunAsync(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Handle cancellation as needed
                        Console.WriteLine("Job was canceled.");
                    }
                }
            });
    }
}