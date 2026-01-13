## Usage

### Option 1: Configuration-Based Setup

Configure Serilog using your `appsettings.json`:# JAC.Framework.Logging.Serilog

A lightweight wrapper for Serilog designed to standardize logging across .NET applications. It simplifies configuration by integrating Serilog with Microsoft.Extensions.Logging and includes pre-configured enrichers for Environment and Thread data, along with Console and File sinks.

## Features

- 🚀 **Easy Integration** - Simple extension methods for service collection configuration
- 🔧 **Flexible Configuration** - Support for both configuration-based and code-based setup
- 📝 **Pre-configured Enrichers** - Includes machine name, thread ID, and log context enrichment
- 📄 **Built-in Sinks** - Console and File sinks configured out of the box
- 🎯 **Standardized Logging** - Consistent logging patterns across your .NET applications