using Autofac;
using Autofac.Extensions.DependencyInjection;
using InterceptorDemo.Application;
using InterceptorDemo.Core;
using InterceptorDemo.Core.CrossCuttingConcerns.Caching;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging.Config;
using InterceptorDemo.Core.Mvc.Extensions;
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
			Configuration = configuration;
			Log.Logger = CreateSerilogLogger(configuration);
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);

			if (Configuration.GetValue<bool>("RedisSettings:UseRedisCache"))
			{
				services.AddDistributedRedisCache(options =>
				{
					options.Configuration = Configuration.GetValue<string>("RedisSettings:Connection");
				});
			}
			else
			{
				//We dont need to register these. Mcv application will automatically register.
				//services.AddMemoryCache();
				//services.AddDistributedMemoryCache();
			}

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
				app.UseAspNetCoreExceptionHandler();
			}
			//app.UseStatusCodePagesWithReExecute("/Errors/Status", "?statusCode={0}");


			app.UseMvc();
		}

		internal IContainer CreateContainer(IServiceCollection services, IConfiguration configuration)
		{
			var config = configuration.GetSection("Logging:CastleCoreSerilogConfig").Get<CastleCoreSerilogConfig>();

			var builder = new ContainerBuilder();
			builder.RegisterModule(new CoreModule(config));
			builder.RegisterModule(new ApplicationModule());
			builder.RegisterModule(new WebAppModule(configuration));

			if (Configuration.GetValue<bool>("RedisSettings:UseRedisCache"))
			{
				builder.RegisterType<DistributedCache>().As<ICache>().SingleInstance();
			}
			else
			{
				// Decide to register type here.

				builder.RegisterType<DistributedCache>().As<ICache>().SingleInstance();
				//builder.RegisterType<InMemoryCache>().As<ICache>().SingleInstance();
			}

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
