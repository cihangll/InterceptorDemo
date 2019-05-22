using Serilog.Events;

namespace InterceptorDemo.Core.CrossCuttingConcerns.Logging.Config
{
	public class SerilogConfig
	{
		public MSSqlServer MSSqlServer { get; set; }
		public File File { get; set; }
	}

	public class CastleCoreSerilogConfig
	{
		public MSSqlServer MSSqlServer { get; set; }
		public File File { get; set; }
	}

	public class MSSqlServer
	{
		public string ConnectionString { get; set; }
		public string TableName { get; set; }
		public LogEventLevel LogEventLevel { get; set; }
	}

	public class File
	{
		public string FullPath { get; set; }
		public LogEventLevel LogEventLevel { get; set; }
	}

}
