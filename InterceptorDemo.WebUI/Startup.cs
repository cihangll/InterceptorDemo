using Autofac;
using Autofac.Extensions.DependencyInjection;
using InterceptorDemo.Application;
using InterceptorDemo.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InterceptorDemo.WebUI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);
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
	}
}
