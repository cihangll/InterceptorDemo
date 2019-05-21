using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ExceptionAttribute : Attribute
	{
		public bool IsActive { get; private set; }

		public ExceptionAttribute(bool isActive = true)
		{
			IsActive = isActive;
		}
	}
}
