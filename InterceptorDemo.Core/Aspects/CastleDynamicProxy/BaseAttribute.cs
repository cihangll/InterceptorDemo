using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class BaseAttribute : Attribute
	{
		public bool IsActive { get; private set; }

		public BaseAttribute(bool isActive)
		{
			IsActive = isActive;
		}
	}
}
