using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects
{
	public abstract class ArgumentValidationAttribute : Attribute
	{
		public abstract void Validate(object value, string argumentName);
	}
}
