using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Caching
{
	public class DistributedCache : ICache
	{
		private readonly IDistributedCache _distributedCache;
		public DistributedCache(IDistributedCache cache)
		{
			_distributedCache = cache;
		}

		public void AddCache(string key, object data, int seconds = 300)
		{
			var byteValue = Encoding.UTF8.GetBytes(data.Serialize());
			_distributedCache.Set(key, byteValue, new DistributedCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds)
			});
		}

		public T GetCache<T>(string key)
		{
			var value = _distributedCache.Get(key);
			if (value != null)
				return Encoding.UTF8.GetString(value).Deserialize<T>();
			return default;
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
			var value = _distributedCache.Get(key);
			if (value != null)
				return Encoding.UTF8.GetString(value).Deserialize<T>();
			AddCache(key, getData.Invoke(), seconds);
			return GetCache<T>(key);
		}

		public void RemoveCache(string key)
		{
			if (IsExist(key))
			{
				_distributedCache.Remove(key);
			}
		}

		public async void AddCacheAsync(string key, object data, int seconds = 300)
		{
			var byteValue = Encoding.UTF8.GetBytes(data.Serialize());

			await _distributedCache.SetAsync(key, byteValue, new DistributedCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds)
			});
		}

		public async Task<T> GetCacheAsync<T>(string key)
		{
			var value = await _distributedCache.GetAsync(key);
			if (value != null)
				return Encoding.UTF8.GetString(value).Deserialize<T>();
			return default;
		}

		public async Task<T> GetCacheAsync<T>(string key, Func<Task<T>> getData, int seconds = 300)
		{
			var value = await _distributedCache.GetAsync(key);
			if (value != null)
				return Encoding.UTF8.GetString(value).Deserialize<T>();

			AddCacheAsync(key, getData.Invoke().Result, seconds);
			return await GetCacheAsync<T>(key);
		}

		public void RemoveCacheAsync(string key)
		{
			_distributedCache.RemoveAsync(key);
		}

		public async Task AddObjectAsync(string key, object obj, int seconds = 300)
		{
			var entryOptions = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds)
			};

			var byteArray = ToByteArray(obj);
			await _distributedCache.SetAsync(key, byteArray, entryOptions);
		}

		public async Task<object> GetObjectAsync(string key)
		{
			var byteArray = await _distributedCache.GetAsync(key);
			return FromByteArray(byteArray);
		}

		private byte[] ToByteArray(object obj)
		{
			if (obj == null)
				return null;

			using (var memoryStream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(memoryStream, obj);
				return memoryStream.ToArray();
			}
		}

		private object FromByteArray(byte[] byteArray)
		{
			if (byteArray == null || byteArray.Length == 0)
				return null;

			using (var memoryStream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				memoryStream.Write(byteArray, 0, byteArray.Length);
				memoryStream.Seek(0, SeekOrigin.Begin);
				return formatter.Deserialize(memoryStream);
			}
		}

		public bool IsExist(string key)
		{
			return string.IsNullOrEmpty(_distributedCache.GetString(key)) ? false : true;
		}
	}
}
