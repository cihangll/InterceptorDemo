using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Caching
{
	public class InMemoryCache : ICache
	{
		private static readonly MemoryCache Cache;
		static InMemoryCache()
		{
			Cache = new MemoryCache(new MemoryCacheOptions() { });
		}

		public void AddCache(string key, object data, int seconds = 300)
		{
			Cache.Set(key, data, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
		}
		public T GetCache<T>(string key)
		{
			if (Cache.TryGetValue(key, out T result))
				return result;
			return result;
		}

		public object GetCache(string key)
		{
			if (Cache.TryGetValue(key, out object result))
				return result;
			return result;
		}

		public T GetCache<T>(string key, Func<T> getData, int seconds = 300)
		{
			if (!Cache.TryGetValue(key, out T _))
			{
				AddCache(key, getData.Invoke(), seconds);
			}
			return GetCache<T>(key);

		}

		public void RemoveCache(string key)
		{
			if (Cache.TryGetValue(key, out _))
				Cache.Remove(key);
		}
		public void AddOrUpdateCache(string key, object data, int seconds = 300)
		{
			Cache.Set(key, data, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
		}

		public bool IsExist(string key)
		{
			return Cache.Get(key) == null ? false : true;
		}


		#region Async

		public void AddCacheAsync(string key, object data, int seconds = 300)
		{
			Cache.Set(key, data,
				new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
		}
		public async Task<T> GetCacheAsync<T>(string key)
		{
			return await Task.Run(() =>
			{
				var data = default(T);
				if (Cache.TryGetValue(key, out data))
					return data;
				return default(T);
			});
		}
		public async Task<T> GetCacheAsync<T>(string key, Func<Task<T>> getData, int seconds = 300)
		{
			if (!Cache.TryGetValue(key, out T _))
			{
				AddCacheAsync(key, getData.Invoke().Result, seconds);
			}
			return await GetCacheAsync<T>(key);

		}

		public async void RemoveCacheAsync(string key)
		{
			await Task.Run(() => Cache.Remove(key));
		}

		public async Task AddObjectAsync(string key, object obj, int seconds = 300)
		{
			await Task.Run(() =>
			{
				Cache.Set(key, obj,
					new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(seconds)));
			});
		}

		public async Task<object> GetObjectAsync(string key)
		{
			return await Task.Run(() =>
			{
				if (Cache.TryGetValue(key, out object data))
					return data;
				return data;
			});
		}

		#endregion
	}
}
