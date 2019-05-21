using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class LogAttribute : Attribute
	{
		public bool IsActive { get; private set; }

		public LogAttribute(bool isActive = true)
		{
			IsActive = isActive;
		}
	}
}
