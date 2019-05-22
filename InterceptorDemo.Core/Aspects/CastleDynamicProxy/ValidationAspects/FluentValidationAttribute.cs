using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ValidationAspects
{
	public class FluentValidationAttribute : BaseAttribute
	{
		public Type ValidatorType { get; private set; }
		public FluentValidationAttribute(Type validatorType = null, bool isActive = true) : base(isActive)
		{
			if (isActive)
			{
				ValidatorType = validatorType ?? throw new ArgumentNullException("ValidatorType cannot be null.");
			}
		}
	}
}
