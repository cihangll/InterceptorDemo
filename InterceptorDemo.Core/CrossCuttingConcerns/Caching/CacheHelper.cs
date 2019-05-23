using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Caching
{
	public static class CacheHelper
	{
		public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
				action(item);
		}

		public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> func, Func<TResult> ifNull = null)
		{
			return (obj != null) && !(obj is DBNull) ? func(obj) : (ifNull != null ? ifNull() : default(TResult));
		}

		public static T To<T>(this object value)
		{
			var result = default(T);
			if (value == null)
				return result;
			if (typeof(T) == typeof(int))
			{
				if (value.GetType().FullName == typeof(bool).FullName)
					return Convert.ToBoolean(value) ? (T)(object)1 : (T)(object)0;
				int a;
				if (int.TryParse(value.ToString(), out a))
					result = (T)(object)a;
			}

			if (typeof(T) == typeof(long))
			{
				long a;
				if (long.TryParse(value.ToString(), out a))
					result = (T)(object)a;
			}


			if (typeof(T) == typeof(short) || typeof(T) == typeof(short?))
			{
				if (value is bool)
					return (T)(object)(short)(Convert.ToBoolean(value) ? 1 : 0);


				short a;
				if (short.TryParse(value.ToString(), out a))
					result = (T)(object)a;
			}

			if (typeof(T) == typeof(string))
			{
				result = (T)value;
			}

			if (typeof(T) == typeof(decimal))
			{
				if (decimal.TryParse(value.ToString(), out decimal a))
					result = (T)(object)a;
			}

			if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
			{
				if (double.TryParse(value.ToString(), out double a))
					result = (T)(object)a;
			}

			if (typeof(T) == typeof(DateTime))
			{
				if (DateTime.TryParse(value.ToString(), out DateTime a))
					result = (T)(object)a;
			}

			if (typeof(T) == typeof(DateTime?))
			{
				if (string.IsNullOrEmpty(value.ToString()))
				{
					return (T)(object)null;
				}

				if (DateTime.TryParse(value.ToString(), out DateTime a))
					result = (T)(object)a;
			}

			if (typeof(T) == typeof(TimeSpan))
			{
				if (TimeSpan.TryParse(value.ToString(), out TimeSpan a))
					result = (T)(object)a;
			}


			if (typeof(T) == typeof(bool))
			{
				if (bool.TryParse(value.ToString(), out bool a))
				{
					result = (T)(object)a;
				}
			}

			return result;
		}

		public static T Deserialize<T>(this string obj)
		{
			return JsonConvert.DeserializeObject<T>(obj);
		}

		public static object Deserialize(this string obj)
		{
			return JsonConvert.DeserializeObject(obj);
		}

		public static string Serialize(this object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		}

		public static string Serialize(this object obj, Formatting formatting)
		{
			return JsonConvert.SerializeObject(obj, formatting, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		}

		public static string ToSha256(this string input)
		{
			using (System.Security.Cryptography.SHA256 md5Hash = System.Security.Cryptography.SHA256.Create())
			{


				var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				var sBuilder = new StringBuilder();
				foreach (byte t in data)
				{
					sBuilder.Append(t.ToString("x2"));
				}
				return sBuilder.ToString();
			}
		}
	}
}
