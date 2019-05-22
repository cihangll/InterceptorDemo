using Castle.DynamicProxy;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy
{
	public enum MethodType
	{
		Synchronous,
		AsyncAction,
		AsyncFunction
	}

	public static class InterceptorHelper
	{
		public static MethodType GetDelegateType(IInvocation invocation)
		{
			var returnType = invocation.Method.ReturnType;
			if (returnType == typeof(Task))
				return MethodType.AsyncAction;
			if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
				return MethodType.AsyncFunction;
			return MethodType.Synchronous;
		}


		public static bool DecideToIntercept<T>(IInvocation invocation, out T attribute) where T : BaseAttribute
		{
			var methodAttribute = AttributeHelper.GetMethodAttribute<T>(invocation);
			if (methodAttribute != null && methodAttribute.IsActive)
			{
				attribute = methodAttribute;
				return true;
			}

			var classAttribute = AttributeHelper.GetClassAttribute<T>(invocation);
			if (classAttribute != null && classAttribute.IsActive)
			{
				attribute = classAttribute;
				return true;
			}

			attribute = null;
			return false;
		}
	}
}
