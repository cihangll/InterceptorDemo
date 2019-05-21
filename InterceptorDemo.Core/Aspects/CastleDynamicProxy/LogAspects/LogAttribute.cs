namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.LogAspects
{

	public class LogAttribute : BaseAttribute
	{
		public LogAttribute(bool isActive = true) : base(isActive) { }
	}
}
