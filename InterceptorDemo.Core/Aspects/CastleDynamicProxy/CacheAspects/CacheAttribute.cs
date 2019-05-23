using System;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.CacheAspects
{
	public class CacheAttribute : BaseAttribute
	{
		public int CacheDurationInSecond { get; private set; }

		public CacheAttribute(int cacheDurationInSecond = 300, bool isActive = true) : base(isActive)
		{
			CacheDurationInSecond = cacheDurationInSecond;
		}
		public CacheAttribute(TimeSpan timeSpan, bool isActive = true) : base(isActive)
		{
			if (timeSpan == null)
			{
				CacheDurationInSecond = 300;
			}
			CacheDurationInSecond = timeSpan.Seconds;
		}
	}
}
