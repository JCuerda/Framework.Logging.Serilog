using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Framework.Logging.Serilog;

public static class LoggerServiceCollectorExtension
{
	public static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration,
		string? applicationName = null)
	{
		var logger = Factory.LoggerFactory.CreateLogger(configuration, applicationName);
		Log.Logger = logger;
		
		services.AddLogging(builder =>
		{
			builder.ClearProviders();
			builder.AddSerilog(logger, dispose: true);
		});
		
		return services;
	}
	
	public static IServiceCollection AddSerilogLogging(this IServiceCollection services, string logFilePath,
		string? applicationName = null)
	{
		var logger = Factory.LoggerFactory.CreateDefaultLogger(applicationName: applicationName ?? string.Empty,
			logFilePath: logFilePath);
		
		Log.Logger = logger;

		services.AddLogging(loggingBuilder =>
		{
			loggingBuilder.ClearProviders();
			loggingBuilder.AddSerilog(logger, dispose: true);
		});

		return services;
	}
}