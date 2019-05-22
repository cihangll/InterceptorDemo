namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects
{
	public class ExceptionHandlingAttribute : BaseAttribute
	{
		public ExceptionHandlingAttribute(bool isActive = true) : base(isActive) { }
	}
}
