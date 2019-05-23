using System;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Caching
{
	public interface ICache
	{
		void AddCache(string key, object data, int seconds = 300);
		T GetCache<T>(string key);
		T GetCache<T>(string key, Func<T> getData, int seconds = 300);
		void RemoveCache(string key);
		void RemoveCacheAsync(string key);
		void AddCacheAsync(string key, object data, int seconds = 300);
		Task<T> GetCacheAsync<T>(string key);
		Task<T> GetCacheAsync<T>(string key, Func<Task<T>> getData, int seconds = 300);
		Task AddObjectAsync(string key, object obj, int seconds = 300);
		Task<object> GetObjectAsync(string key);
	}
}
