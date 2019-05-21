

using Castle.Core.Internal;
using Castle.DynamicProxy;
using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy
{
	public static class AttributeHelper
	{
		public static T GetClassAttribute<T>(IInvocation invocation) where T : Attribute
		{
			return invocation.TargetType.GetAttribute<T>();
		}

		public static T GetMethodAttribute<T>(IInvocation invocation) where T : Attribute
		{
			return invocation.MethodInvocationTarget.GetAttribute<T>();
		}

		public static bool IsDefinedClassLevel<T>(IInvocation invocation) where T : Attribute
		{
			return invocation.TargetType.IsDefined(typeof(T), false);
		}

		public static bool IsDefinedMethodLevel<T>(IInvocation invocation) where T : Attribute
		{
			return invocation.MethodInvocationTarget.IsDefined(typeof(T), false);
		}
	}
}
