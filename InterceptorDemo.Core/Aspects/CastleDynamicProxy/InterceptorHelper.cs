using Castle.DynamicProxy;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy
{
	public static class InterceptorHelper
	{
		public static bool DecideToRun<T>(IInvocation invocation) where T : BaseAttribute
		{
			//TODO: rewrite here.
			var classAttribute = AttributeHelper.GetClassAttribute<T>(invocation);
			var methodAttribute = AttributeHelper.GetMethodAttribute<T>(invocation);
			return true;
		}
	}
}
