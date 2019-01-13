using System;
using System.Globalization;
using System.Reflection;
using log4net;
using log4net.Core;

namespace MySqlConnector.Logging
{
	public sealed class Log4netLoggerProvider : IMySqlConnectorLoggerProvider
	{
		public IMySqlConnectorLogger CreateLogger(string name) => new Log4netLogger(LogManager.GetLogger(s_loggerAssembly, "MySqlConnector." + name));

		static readonly Assembly s_loggerAssembly = typeof(Log4netLogger).GetTypeInfo().Assembly;
		static readonly Type s_loggerType = typeof(Log4netLogger);

		private class Log4netLogger : IMySqlConnectorLogger
		{
			public Log4netLogger(ILoggerWrapper log) => m_logger = log.Logger;

			public bool IsEnabled(MySqlConnectorLogLevel level) => m_logger.IsEnabledFor(GetLevel(level));

			public void Log(MySqlConnectorLogLevel level, string message, object[] args = null, Exception exception = null)
			{
				if (args == null || args.Length == 0)
					m_logger.Log(s_loggerType, GetLevel(level), message, exception);
				else
					m_logger.Log(s_loggerType, GetLevel(level), string.Format(CultureInfo.InvariantCulture, message, args), exception);
			}

			private static Level GetLevel(MySqlConnectorLogLevel level)
			{
				switch (level)
				{
				case MySqlConnectorLogLevel.Trace:
					return Level.Trace;
				case MySqlConnectorLogLevel.Debug:
					return Level.Debug;
				case MySqlConnectorLogLevel.Info:
					return Level.Info;
				case MySqlConnectorLogLevel.Warn:
					return Level.Warn;
				case MySqlConnectorLogLevel.Error:
					return Level.Error;
				case MySqlConnectorLogLevel.Fatal:
					return Level.Fatal;
				default:
					throw new ArgumentOutOfRangeException(nameof(level), level, "Invalid value for 'level'.");
				}
			}

			readonly ILogger m_logger;
		}
	}
}
