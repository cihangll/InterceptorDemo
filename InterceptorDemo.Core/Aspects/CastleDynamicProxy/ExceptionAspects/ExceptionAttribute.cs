namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects
{
	public class ExceptionAttribute : BaseAttribute
	{
		public ExceptionAttribute(bool isActive = true) : base(isActive) { }
	}
}
