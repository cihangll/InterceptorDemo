using Castle.Services.Logging.SerilogIntegration;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging.Config;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Logging.CastleCoreSerilog
{
	public static class SerilogInstance
	{
		public static Castle.Core.Logging.ILogger CreateCastleCoreLogger(CastleCoreSerilogConfig config)
		{
			var loggerConfig = new LoggerConfiguration();
			if (config.MSSqlServer != null && !string.IsNullOrEmpty(config.MSSqlServer.ConnectionString))
			{
				loggerConfig.WriteTo.MSSqlServer(config.MSSqlServer.ConnectionString, config.MSSqlServer.TableName, config.MSSqlServer.LogEventLevel, 50, null, null, true);
			}

			if (config.File != null && !string.IsNullOrEmpty(config.File.FullPath))
			{
				loggerConfig.WriteTo.File(config.File.FullPath, config.File.LogEventLevel, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true);
			}

			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
			{
				loggerConfig.WriteTo.Debug(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}");
			}

			var serilogLogger = loggerConfig.CreateLogger();

			var serilogFactory = new SerilogFactory(serilogLogger);

			var logger = serilogFactory.Create("castle core logger");
			return logger;
		}
	}
}
