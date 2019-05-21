using Autofac;
using Autofac.Extensions.DependencyInjection;
using Castle.Services.Logging.SerilogIntegration;
using InterceptorDemo.Application;
using InterceptorDemo.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace InterceptorDemo.WebUI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Log.Logger = CreateSerilogLogger(configuration);
			Configuration = configuration;
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);

			services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

			services.AddMvc();

			var container = CreateContainer(services, Configuration);
			return new AutofacServiceProvider(container);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDatabaseErrorPage();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				throw new NotSupportedException("This case cannot supported.");
			}

			app.UseMvc();
		}

		internal IContainer CreateContainer(IServiceCollection services, IConfiguration configuration)
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule(new CoreModule());
			builder.RegisterModule(new ApplicationModule());
			builder.RegisterModule(new WebAppModule(configuration));

			builder.Populate(services);

			return builder.Build();
		}

		internal Castle.Core.Logging.ILogger CreateCastleCoreLogger(IConfiguration configuration)
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

		internal static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
		{
			var loggerConfig = new LoggerConfiguration()
				.WriteTo.MSSqlServer(configuration.GetConnectionString("Default"), "Logs", Serilog.Events.LogEventLevel.Error, 50, null, null, true, null, "dbo");
			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
			{
				loggerConfig.WriteTo.Debug(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}");
			}

			var serilogLogger = loggerConfig.CreateLogger();
			return Log.Logger = serilogLogger;
		}
	}
}
