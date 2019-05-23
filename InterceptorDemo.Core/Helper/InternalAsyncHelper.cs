﻿using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.Helper
{
	/// <summary>
	/// More info https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.Core/Volo/Abp/Threading/InternalAsyncHelper.cs
	/// https://stackoverflow.com/a/28374134/8069766
	/// </summary>
	public static class InternalAsyncHelper
	{
		public static bool IsAsyncMethod(MethodInfo method)
		{
			return (
				method.ReturnType == typeof(Task) ||
				(method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
				);
		}



		public static async Task AwaitTaskWithFinally(Task actualReturnValue, Action<Exception> finalAction)
		{
			Exception exception = null;

			try
			{
				await actualReturnValue;
			}
			catch (Exception ex)
			{
				exception = ex;
				throw;
			}
			finally
			{
				finalAction(exception);
			}
		}

		public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
		{
			Exception exception = null;

			try
			{
				await actualReturnValue;
				await postAction();
			}
			catch (Exception ex)
			{
				exception = ex;
				throw;
			}
			finally
			{
				finalAction(exception);
			}
		}

		public static async Task AwaitTaskWithPreActionAndPostActionAndFinally(Func<Task> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
		{
			Exception exception = null;

			try
			{
				if (preAction != null)
				{
					await preAction();
				}

				await actualReturnValue();

				if (postAction != null)
				{
					await postAction();
				}
			}
			catch (Exception ex)
			{
				exception = ex;
				throw;
			}
			finally
			{
				if (finalAction != null)
				{
					finalAction(exception);
				}
			}
		}

		public static async Task<T> AwaitTaskWithFinallyAndGetResult<T>(Task<T> actualReturnValue, Action<Exception> finalAction)
		{
			Exception exception = null;

			try
			{
				return await actualReturnValue;
			}
			catch (Exception ex)
			{
				exception = ex;
				throw;
			}
			finally
			{
				finalAction(exception);
			}
		}

		public static object CallAwaitTaskWithFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Action<Exception> finalAction)
		{
			return typeof(InternalAsyncHelper)
				.GetTypeInfo()
				.GetMethod("AwaitTaskWithFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
				.MakeGenericMethod(taskReturnType)
				.Invoke(null, new object[] { actualReturnValue, finalAction });
		}

		public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
		{
			Exception exception = null;

			try
			{
				var result = await actualReturnValue;
				await postAction();
				return result;
			}
			catch (Exception ex)
			{
				exception = ex;
				throw;
			}
			finally
			{
				finalAction(exception);
			}
		}

		public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<Exception> finalAction)
		{
			return typeof(InternalAsyncHelper)
				.GetTypeInfo()
				.GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
				.MakeGenericMethod(taskReturnType)
				.Invoke(null, new object[] { actualReturnValue, action, finalAction });
		}

		public static async Task<T> AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult<T>(Func<Task<T>> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
		{
			Exception exception = null;

			try
			{
				if (preAction != null)
				{
					await preAction();
				}

				var result = await actualReturnValue();

				if (postAction != null)
				{
					await postAction();
				}

				return result;
			}
			catch (Exception ex)
			{
				exception = ex;
				throw;
			}
			finally
			{
				finalAction?.Invoke(exception);
			}
		}

		public static object CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(Type taskReturnType, Func<object> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
		{
			var returnFunc = typeof(InternalAsyncHelper)
				.GetTypeInfo()
				.GetMethod("ConvertFuncOfObjectToFuncOfTask", BindingFlags.NonPublic | BindingFlags.Static)
				.MakeGenericMethod(taskReturnType)
				.Invoke(null, new object[] { actualReturnValue });

			return typeof(InternalAsyncHelper)
				.GetTypeInfo()
				.GetMethod("AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
				.MakeGenericMethod(taskReturnType)
				.Invoke(null, new object[] { returnFunc, preAction, postAction, finalAction });
		}

		private static Func<Task<T>> ConvertFuncOfObjectToFuncOfTask<T>(Func<object> actualReturnValue)
		{
			return () => (Task<T>)actualReturnValue();
		}
	}
}