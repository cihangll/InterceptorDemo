using Autofac;
using Autofac.Extensions.DependencyInjection;
using InterceptorDemo.Application;
using InterceptorDemo.Core;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace InterceptorDemo.WebUI
{

	public class EmailSettings
	{
		public string EmailTemplatesPath { get; set; }
		public string Email { get; set; }
	}

	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			Log.Logger = CreateSerilogLogger(configuration);
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);

			var serilogConfig = new SerilogConfig();
			Configuration.GetSection("Logging:SerilogConfig").Bind(serilogConfig);
			services.AddSingleton(serilogConfig);

			var castleCoreSerilogConfig = new CastleCoreSerilogConfig();
			Configuration.GetSection("Logging:CastleCoreSerilogConfig").Bind(castleCoreSerilogConfig);
			services.AddSingleton(castleCoreSerilogConfig);

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
			var config = configuration.GetSection("Logging:CastleCoreSerilogConfig").Get<CastleCoreSerilogConfig>();

			var builder = new ContainerBuilder();
			builder.RegisterModule(new CoreModule(config));
			builder.RegisterModule(new ApplicationModule());
			builder.RegisterModule(new WebAppModule(configuration));

			builder.Populate(services);

			return builder.Build();
		}

		internal static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
		{
			var config = configuration.GetSection("Logging:SerilogConfig").Get<SerilogConfig>();

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
			return Log.Logger = serilogLogger;
		}
	}
}
