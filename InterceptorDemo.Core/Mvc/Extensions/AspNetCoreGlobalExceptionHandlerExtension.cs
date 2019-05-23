using Castle.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using System;

namespace InterceptorDemo.Core.Mvc.Extensions
{
	public static class AspNetCoreGlobalExceptionHandlerExtension
	{
		public static IApplicationBuilder UseAspNetCoreExceptionHandler(this IApplicationBuilder app)
		{
			var logger = app.ApplicationServices.GetService(typeof(ILogger)) as ILogger;
			return app.UseExceptionHandler(HandleException(logger));
		}

		public static Action<IApplicationBuilder> HandleException(ILogger logger)
		{
			return appBuilder =>
			{
				appBuilder.Run(async context =>
				{
					var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

					if (exceptionHandlerFeature != null)
					{
						logger.Error(exceptionHandlerFeature.Error.Message, exceptionHandlerFeature.Error);
					}

					context.Response.Redirect($"/Errors/Status?statusCode={context.Response.StatusCode}");
				});
			};
		}
	}
}
