namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.MeasureAspects
{
	public class MeasureDurationAttribute : BaseAttribute
	{
		public int LogAfterSeconds { get; private set; }
		public MeasureDurationAttribute(int logAfterSeconds = 5, bool isActive = true) : base(isActive)
		{
			LogAfterSeconds = logAfterSeconds;
		}
	}
}
