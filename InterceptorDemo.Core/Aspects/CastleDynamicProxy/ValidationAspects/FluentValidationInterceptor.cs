using Castle.Core.Logging;
using Castle.DynamicProxy;
using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects
{
	[Serializable]
	public class FluentValidationInterceptor : IInterceptor
	{
		private static readonly MethodInfo handleAsyncMethodInfo = typeof(FluentValidationInterceptor).GetMethod("InterceptAsynchronousWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
		private FluentValidationAttribute attribute;

		private readonly ILogger _logger;

		public FluentValidationInterceptor(ILogger logger)
		{
			_logger = logger;
		}

		public void Intercept(IInvocation invocation)
		{
			if (!InterceptorHelper.DecideToIntercept(invocation, out attribute))
			{
				invocation.Proceed();
				return;
			}

			var validator = (IValidator)Activator.CreateInstance(attribute.ValidatorType);
			var entityType = attribute.ValidatorType.BaseType.GetGenericArguments()[0];

			var entities = invocation.Arguments.Where(t => t != null && t.GetType() == entityType).ToList();
			foreach (var entity in entities)
			{
				var result = validator.Validate(entity);
				if (result.Errors.Count > 0)
				{
					_logger.Info(() =>
					{
						return JsonConvert.SerializeObject(result.Errors.ToList().Select(e => $"{e.ErrorCode} - {e.ErrorMessage}").ToList(), Formatting.None);
					});
					throw new ValidationException(result.Errors);
				}
			}

			invocation.Proceed();
		}
	}
}
