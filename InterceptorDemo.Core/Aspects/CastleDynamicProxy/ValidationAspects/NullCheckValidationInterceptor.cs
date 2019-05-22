using Castle.DynamicProxy;
using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects
{
	[Serializable]
	public class NullCheckValidationInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			var parameters = invocation.MethodInvocationTarget.GetParameters();
			for (int index = 0; index < parameters.Length; index++)
			{
				var paramInfo = parameters[index];
				var attributes = paramInfo.GetCustomAttributes(typeof(ArgumentValidationAttribute), false);

				if (attributes.Length == 0)
					continue;

				foreach (ArgumentValidationAttribute attr in attributes)
				{
					attr.Validate(invocation.Arguments[index], paramInfo.Name);
				}
			}

			invocation.Proceed();
		}
	}
}
