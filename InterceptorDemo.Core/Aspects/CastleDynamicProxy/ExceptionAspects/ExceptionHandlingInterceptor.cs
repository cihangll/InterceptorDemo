using Castle.Core.Logging;
using Castle.DynamicProxy;
using InterceptorDemo.Core.CrossCuttingConcerns.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace InterceptorDemo.Core.Aspects.CastleDynamicProxy.ExceptionAspects
{
	[Serializable]
	public class ExceptionHandlingInterceptor : IInterceptor
	{
		private static readonly MethodInfo handleAsyncMethodInfo = typeof(ExceptionHandlingInterceptor).GetMethod("InterceptAsynchronousWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
		private ExceptionHandlingAttribute attribute;

		private readonly ILogger _logManager;

		public ExceptionHandlingInterceptor(ILogger logManager)
		{
			_logManager = logManager;
		}

		public void Intercept(IInvocation invocation)
		{

			if (!InterceptorHelper.DecideToIntercept(invocation, out attribute))
			{
				invocation.Proceed();
				return;
			}

			var delegateType = InterceptorHelper.GetDelegateType(invocation);
			if (delegateType == MethodType.Synchronous)
			{
				InterceptSynchronous(invocation);
			}
			if (delegateType == MethodType.AsyncAction)
			{
				InterceptAsynchronous(invocation);
			}
			if (delegateType == MethodType.AsyncFunction)
			{
				InterceptAsynchronousWithResultUsingReflection(invocation);
			}
		}

		private void InterceptSynchronous(IInvocation invocation)
		{
			try
			{
				invocation.Proceed();
			}
			catch (ArgumentNullException e)
			{
				_logManager.Error($"Argument cannot be null.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
			catch (Exception e)
			{
				_logManager.Error($"Error occured.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
		}

		private void InterceptAsynchronous(IInvocation invocation)
		{
			invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
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

		private async Task InternalInterceptAsynchronous(IInvocation invocation)
		{
			try
			{
				invocation.Proceed();
				var task = (Task)invocation.ReturnValue;
				await task;
			}
			catch (ArgumentNullException e)
			{
				_logManager.Error($"Argument cannot be null.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
			catch (Exception e)
			{
				_logManager.Error($"Error occured.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
		}

		private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
		{
			try
			{
				invocation.Proceed();
				var task = (Task<TResult>)invocation.ReturnValue;
				TResult result = await task;

				return result;
			}
			catch (ArgumentNullException e)
			{
				_logManager.Error($"Argument cannot be null.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
			catch (Exception e)
			{
				_logManager.Error($"Error occured.{Environment.NewLine}{LogHelper.CreateLogString(invocation)}", e);
				throw;
			}
		}
	}
}
