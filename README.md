## Features

- 🚀 **Easy integration** - Simple extension methods for registering Serilog with `IServiceCollection`
- 🔧 **Flexible configuration** - Supports both `appsettings.json`-based and code-based setup
- 📝 **Built-in enrichment** - Adds machine name, thread ID, and log context enrichment
- 📄 **Ready-to-use sinks** - Console and file logging available out of the box
- 🎯 **Consistent logging** - Standardized logging configuration across .NET applications

## Installation

Install the NuGet package:
```powershell 
dotnet add package JAC.Framework.Logging.Serilog
```

## Quick Start

Register logging in your application using one of the provided extension methods.

### Using `IConfiguration`

Use this option when you want Serilog settings to come from `appsettings.json` or another .NET configuration provider.

```csharp 
using Framework.Logging.Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilogLogging( builder.Configuration, applicationName: "MyApp");
```
### Using the default code-based configuration

Use this option when you want a simple built-in setup with console and file logging.

```csharp
csharp using Framework.Logging.Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilogLogging( logFilePath: "logs/app-.txt", applicationName: "MyApp");
```
## JSON Configuration

The `AddSerilogLogging(IServiceCollection, IConfiguration, string?)` overload reads settings from the top-level `Serilog` section using `ReadFrom.Configuration(...)` [[1]](https://github.com/serilog/serilog-settings-configuration).

### Example `appsettings.json`
```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Sinks.Async"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Async",
        "Args": {
          "configure": [
            {
              "Name": "Console",
              "Args": {
                "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{SourceContext}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"
              }
            },
            {
              "Name": "File",
              "Args": {
                "path": "logs/imdotnet-.log",
                "rollingInterval": "Day",
                "retainedFileCountLimit": 30,
                "restrictedToMinimumLevel": "Warning",
                "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [{SourceContext}] [{Server}] {Message:lj}{NewLine}{Exception}"
              }
            }
          ]
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId"],
    "Properties": {
      "Application": "MyApp"
      "Server": "Server: localhost"
    }
  }
}
 ```
### Registration with configuration

```csharp
csharp using Framework.Logging.Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilogLogging( builder.Configuration, applicationName: "MyApp");
```

### Important notes

- The `Serilog` section must exist at the top level of the configuration source [[1]](https://github.com/serilog/serilog-settings-configuration).
- The `Using` section explicitly declares the sinks and enrichers used by JSON configuration [[1]](https://github.com/serilog/serilog-settings-configuration).
- When `applicationName` is provided, the library enriches log events with an application-specific property in code.
- JSON-based configuration is useful when you want to adjust log levels, sinks, or enrichment without changing source code.

## Code-Based Configuration

The `AddSerilogLogging(IServiceCollection, string, string?)` overload creates a default logger with:

- Minimum level set to `Information`
- `Microsoft` logs overridden to `Warning`
- `System` logs overridden to `Warning`
- Log context enrichment enabled
- Machine name enrichment enabled
- Thread ID enrichment enabled
- Console sink enabled
- Rolling file sink enabled

### Example

```csharp
using Framework.Logging.Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilogLogging( logFilePath: "logs/app-.txt", applicationName: "MyApp");
```

## Default Output Behavior

The built-in code-based configuration writes logs to:

- **Console**
- **Rolling file logs** using a daily interval

Default file path:
```text
logs/app-{Date}.txt
```

Default file retention:
```text 
30 files
```
## Enrichment

This package adds the following enrichers:

- `FromLogContext`
- `WithMachineName`
- `WithThreadId`

When an application name is provided, an application property is also attached to log events.

## Example Usage

After registration, continue using the standard `ILogger<T>` abstraction from `Microsoft.Extensions.Logging`.

```csharp
csharp using Microsoft.Extensions.Logging;
public sealed class Worker(ILogger<Worker> logger) 
{
    public void Run() 
    {
        logger.LogInformation("Worker started at {StartedAt}", DateTimeOffset.UtcNow);
    }
}
```
## Requirements

- .NET 8.0

## License

MIT