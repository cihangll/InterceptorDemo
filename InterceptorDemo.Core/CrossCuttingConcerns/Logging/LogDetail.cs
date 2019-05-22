using Newtonsoft.Json;
using System.Collections.Generic;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Logging
{
	public class LogDetail
	{
		public string FullName { get; set; }
		public string MethodName { get; set; }
		public List<LogParameter> Parameters { get; set; }

		internal string ToJsonString()
		{
			return JsonConvert.SerializeObject(this, new JsonSerializerSettings
			{
				Formatting = Formatting.None,
				NullValueHandling = NullValueHandling.Include
			});
		}
	}
}
