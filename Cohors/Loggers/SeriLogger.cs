using Cohors.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Cohors.Loggers;

/// <summary>
///     Provides a configuration setup for Serilog, a logging library, within a .NET Core application.
/// </summary>
public static class SeriLogger
{
    /// <summary>
    ///     Configuration action that sets up Serilog. Enriches log events, specifies output destinations,
    ///     and connects to an Elasticsearch instance.
    /// </summary>
    /// <remarks>
    ///     Ensure the following configuration options are present in your appsettings.json:
    ///     <code>
    /// "Serilog": {
    ///    "MinimumLevel": {
    ///      "Default": "Information",
    ///      "Override": {
    ///        "Microsoft": "Information",
    ///        "System": "Warning"
    ///      }
    ///    }
    ///  },
    ///  "ElasticConfiguration": {
    ///    "Uri": "http://localhost:9200"
    ///  }
    /// </code>
    /// </remarks>
    /// <param name="context">The host builder context which contains the configuration and hosting environment.</param>
    /// <param name="configuration">The logger configuration to be enriched and set up.</param>
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, configuration) =>
    {
        
            // Retrieve the Elastic URI from the configuration
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

            // If the Elastic URI is missing or empty, throw an exception
            if (string.IsNullOrEmpty(elasticUri))
                throw new ConfigurationMissingException("ElasticConfiguration:Uri");

            // Configure the logger
            configuration
                // Enrich log events with contextual information
                .Enrich.FromLogContext()
                // Enrich log events with the machine name
                .Enrich.WithMachineName()
                // Write log events to the debug output
                .WriteTo.Debug()
                // Write log events to the console
                .WriteTo.Console()
                // Write log events to an Elasticsearch instance
                .WriteTo.Elasticsearch(
                   new ElasticsearchSinkOptions(new Uri(elasticUri))
                   {
                       // Set the index format for the Elasticsearch instance
                       IndexFormat =
                           $"applogs-{context.HostingEnvironment.ApplicationName.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                       // Automatically register the template with Elasticsearch
                       AutoRegisterTemplate = true,
                       // Set the number of shards for the Elasticsearch instance
                       NumberOfShards = 2,
                       // Set the number of replicas for the Elasticsearch instance
                       NumberOfReplicas = 1
                   })
                // Enrich log events with the environment name
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                // Enrich log events with the application name
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                // Read the logger configuration from the provided configuration
                .ReadFrom.Configuration(context.Configuration);
    };
}