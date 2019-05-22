using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class NotNullAttribute : ArgumentValidationAttribute
	{
		public override void Validate(object value, string argumentName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}
