using Castle.DynamicProxy;
using System.Linq;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Logging
{
	public static class LogHelper
	{
		public static string CreateLogString(IInvocation invocation)
		{
			var logParameters = invocation.Method.GetParameters().Select((t, i) => new LogParameter
			{
				Name = t.Name,
				Type = t.ParameterType.Name,
				Value = invocation.Arguments[i]
			}).ToList();

			var logDetail = new LogDetail
			{
				FullName = invocation.Method.DeclaringType?.Name,
				MethodName = invocation.Method.Name,
				Parameters = logParameters
			};

			return logDetail.ToJsonString();
		}
	}
}
