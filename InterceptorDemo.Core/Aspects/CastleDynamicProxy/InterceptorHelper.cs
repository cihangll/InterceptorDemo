using Castle.DynamicProxy;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy
{
	public static class InterceptorHelper
	{
		public static bool DecideToIntercept<T>(IInvocation invocation) where T : BaseAttribute
		{
			var methodAttribute = AttributeHelper.GetMethodAttribute<T>(invocation);
			if (methodAttribute != null && methodAttribute.IsActive)
			{
				return true;
			}

			var classAttribute = AttributeHelper.GetClassAttribute<T>(invocation);
			if (classAttribute != null && classAttribute.IsActive)
			{
				return true;
			}

			return false;
		}
	}
}
