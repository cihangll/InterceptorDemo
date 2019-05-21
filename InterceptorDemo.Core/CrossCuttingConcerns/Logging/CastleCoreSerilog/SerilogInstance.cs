using Castle.Services.Logging.SerilogIntegration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Logging.CastleCoreSerilog
{
	public static class SerilogInstance
	{
		public static Castle.Core.Logging.ILogger CreateCastleCoreLogger(IConfiguration configuration)
		{
			var loggerConfig = new LoggerConfiguration()
				.WriteTo.MSSqlServer(configuration.GetConnectionString("Default"), "Logs", Serilog.Events.LogEventLevel.Error, 50, null, null, true, null, "dbo");
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
