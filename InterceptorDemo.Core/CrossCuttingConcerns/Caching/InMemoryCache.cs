using Microsoft.Extensions.Caching.Memory;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Caching
{
	public class InMemoryCache : ICache
	{
		private readonly IMemoryCache _memoryCache;

		public InMemoryCache(IMemoryCache cache)
		{
			_memoryCache = cache;
		}

		public void AddCache(string key, object data, int seconds = 300)
		{
			_memoryCache.Set(key, data, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
		}
		public T GetCache<T>(string key)
		{
			if (_memoryCache.TryGetValue(key, out T result))
				return result;
			return result;
		}

		public object GetCache(string key, Type resultType)
		{
			var methodInfo = GetType().GetMethod("GetCache", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
			var genericMethodInfo = methodInfo.MakeGenericMethod(resultType);
			object value = genericMethodInfo.Invoke(this, new[] { key });
			if (value != null)
				return value;
			return default;
		}

		public T GetCache<T>(string key, Func<T> getData, int seconds = 300)
		{
			if (!_memoryCache.TryGetValue(key, out T _))
			{
				AddCache(key, getData.Invoke(), seconds);
			}
			return GetCache<T>(key);

		}

		public void RemoveCache(string key)
		{
			if (_memoryCache.TryGetValue(key, out _))
				_memoryCache.Remove(key);
		}
		public void AddOrUpdateCache(string key, object data, int seconds = 300)
		{
			_memoryCache.Set(key, data, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
		}

		public bool IsExist(string key)
		{
			return _memoryCache.Get(key) == null ? false : true;
		}


		#region Async

		public void AddCacheAsync(string key, object data, int seconds = 300)
		{
			_memoryCache.Set(key, data,
				new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
		}
		public async Task<T> GetCacheAsync<T>(string key)
		{
			return await Task.Run(() =>
			{
				var data = default(T);
				if (_memoryCache.TryGetValue(key, out data))
					return data;
				return default(T);
			});
		}
		public async Task<T> GetCacheAsync<T>(string key, Func<Task<T>> getData, int seconds = 300)
		{
			if (!_memoryCache.TryGetValue(key, out T _))
			{
				AddCacheAsync(key, getData.Invoke().Result, seconds);
			}
			return await GetCacheAsync<T>(key);

		}

		public async void RemoveCacheAsync(string key)
		{
			await Task.Run(() => _memoryCache.Remove(key));
		}

		public async Task AddObjectAsync(string key, object obj, int seconds = 300)
		{
			await Task.Run(() =>
			{
				_memoryCache.Set(key, obj,
					new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
			});
		}

		public async Task<object> GetObjectAsync(string key)
		{
			return await Task.Run(() =>
			{
				if (_memoryCache.TryGetValue(key, out object data))
					return data;
				return data;
			});
		}

		#endregion
	}
}
