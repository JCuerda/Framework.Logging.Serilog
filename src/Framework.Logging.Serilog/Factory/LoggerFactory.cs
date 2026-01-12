using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Framework.Logging.Serilog.Factory;

/// <summary>
/// Factory class for creating and configuring Serilog logger instances.
/// </summary>
public static class LoggerFactory
{
	/// <summary>
	/// Creates a logger configured from the provided configuration.
	/// </summary>
	/// <param name="configuration">Application configuration containing Serilog settings.</param>
	/// <param name="applicationName">Optional application name for context enrichment.</param>
	/// <returns>Configured Serilog logger.</returns>
	public static ILogger CreateLogger(IConfiguration configuration, string? applicationName = null)
	{
		var loggerConfiguration = new LoggerConfiguration()
			.ReadFrom.Configuration(configuration);
		
		if(!string.IsNullOrWhiteSpace(applicationName))
		{
			loggerConfiguration.Enrich.WithProperty("ApplicationName", applicationName);
		}

		// Add Serilog Enrichers
		loggerConfiguration.Enrich.FromLogContext()
			.Enrich.WithMachineName()
			.Enrich.WithThreadId();
		
		return loggerConfiguration.CreateLogger();
	}
	
	/// <summary>
	/// Creates a logger with default console and file sinks.
	/// </summary>
	/// <param name="applicationName">Application name for context enrichment.</param>
	/// <param name="minimumLevel">Minimum log level. Defaults to Information.</param>
	/// <param name="logFilePath">Path for a log file. Defaults to logs/app-.txt</param>
	/// <returns>Configured Serilog logger.</returns>
	public static ILogger CreateDefaultLogger(
		string applicationName,
		LogEventLevel minimumLevel = LogEventLevel.Information,
		string logFilePath = "logs/app-.txt")
	{
		return new LoggerConfiguration()
			.MinimumLevel.Is(minimumLevel)
			.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
			.MinimumLevel.Override("System", LogEventLevel.Warning)
			.Enrich.FromLogContext()
			.Enrich.WithProperty("Application", applicationName)
			.Enrich.WithMachineName()
			.Enrich.WithThreadId()
			.WriteTo.Console(
				outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
			.WriteTo.File(
				logFilePath,
				rollingInterval: RollingInterval.Day,
				outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{Application}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
				retainedFileCountLimit: 30)
			.CreateLogger();
	}
}