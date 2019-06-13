using Castle.DynamicProxy;
using InterceptorDemo.Core.CrossCuttingConcerns.Caching;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.CacheAspects
{
	[Serializable]
	public class CacheInterceptor : IInterceptor
	{
		private static readonly MethodInfo handleAsyncMethodInfo = typeof(CacheInterceptor).GetMethod("InterceptAsynchronousWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
		private CacheAttribute attribute;
		private string cacheKey;

		private readonly ICache _cache;

		public CacheInterceptor(ICache cache)
		{
			_cache = cache;
		}

		public void Intercept(IInvocation invocation)
		{
			if (!InterceptorHelper.DecideToIntercept(invocation, out attribute)
				|| invocation.Method.ReturnType == typeof(void)
				|| invocation.Method.ReturnType == typeof(Task))
			{
				invocation.Proceed();
				return;
			}

			cacheKey = string.Concat(invocation.TargetType.FullName, ".", invocation.Method.Name, "(", JsonConvert.SerializeObject(invocation.Arguments), ")");

			var delegateType = InterceptorHelper.GetDelegateType(invocation);
			if (delegateType == MethodType.Synchronous)
			{
				InterceptSynchronous(invocation);
			}
			if (delegateType == MethodType.AsyncAction)
			{
				throw new NotSupportedException($"A method with a return type Task cannot be cached.");
			}
			if (delegateType == MethodType.AsyncFunction)
			{
				InterceptAsynchronousWithResultUsingReflection(invocation);
			}
		}

		private void InterceptSynchronous(IInvocation invocation)
		{
			if (_cache.IsExist(cacheKey))
			{
				var cacheResult = GetCache(invocation.Method.ReturnType);
				if (cacheResult != null && cacheResult.GetType() == invocation.Method.ReturnType)
				{
					invocation.ReturnValue = cacheResult;
				}
				else
				{
					SaveCache();
				}
			}
			else
			{
				SaveCache();
			}

			void SaveCache()
			{
				invocation.Proceed();
				AddCache(invocation.ReturnValue);
			}
		}

		private void InterceptAsynchronousWithResult<TResult>(IInvocation invocation)
		{
			invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
		}

		private void InterceptAsynchronousWithResultUsingReflection(IInvocation invocation)
		{
			var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
			var methodInfo = handleAsyncMethodInfo.MakeGenericMethod(resultType);
			methodInfo.Invoke(this, new[] { invocation });
		}

		private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
		{
			if (_cache.IsExist(cacheKey))
			{
				var cacheResult = GetCache(invocation.Method.ReturnType.GetGenericArguments()[0]);
				if (cacheResult != null && cacheResult.GetType() == typeof(TResult))
				{
					return (TResult)cacheResult;
				}
				else
				{
					return await SaveCache();
				}
			}
			else
			{
				return await SaveCache();
			}

			async Task<TResult> SaveCache()
			{
				invocation.Proceed();
				var task = (Task<TResult>)invocation.ReturnValue;
				TResult result = await task;

				AddCache(result);
				return result;
			}
		}

		private void AddCache(object data)
		{
			if (data == null)
			{
				return;
			}
			_cache.RemoveCache(cacheKey);
			_cache.AddCache(cacheKey, data, attribute.CacheDurationInSecond);
		}

		private object GetCache(Type type)
		{
			return _cache.GetCache(cacheKey, type);
		}
	}
}
