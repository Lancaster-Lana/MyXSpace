using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace MyXSpace.Core.Logging
{
    public class FileLogger : ILogger
    {
        private string filePath;
        private object _lock = new object();
        public FileLogger(string path)
        {
            filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }
    }


    /*
     public class Logger : ILogger
	{
		public const string CONFIG_FILE_NAME = "Log4Net.config";

		private static readonly Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();
		Action<object, object> info = (source, message) => getLogger(source.GetType()).Info(message);
		Action<object, object> warn = (source, message) => getLogger(source.GetType()).Warn(message);
		Action<object, object> error = (source, message) => getLogger(source.GetType()).Error(message);
		Action<object, object> fatal = (source, message) => getLogger(source.GetType()).Fatal(message);
		Action<object, object> debug = (source, message) => getLogger(source.GetType()).Debug(message);

		private static bool _logInitialized;
		private void initialize()
		{
			XmlConfigurator.ConfigureAndWatch(new FileInfo(getConfigFilePath()));
		}

		private string getConfigFilePath()
		{
			string basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			string configPath = Path.Combine(basePath, CONFIG_FILE_NAME);

			if (!File.Exists(configPath))
			{
				configPath = Path.Combine(basePath, "bin");
				configPath = Path.Combine(configPath, CONFIG_FILE_NAME);

				if (!File.Exists(configPath))
				{
					configPath = Path.Combine(basePath, @"..\" + CONFIG_FILE_NAME);
				}
			}

			return configPath;
		}

		public void EnsureInitialized()
		{
			if (!_logInitialized)
			{
				initialize();
				_logInitialized = true;
			}
		}

		public string SerializeException(Exception e)
		{
			return serializeException(e, string.Empty);
		}

		private string serializeException(Exception e, string exceptionMessage)
		{
			if (e == null) return string.Empty;

			exceptionMessage = string.Format(
				"{0}{1}{2}\n{3}",
				exceptionMessage,
				(exceptionMessage == string.Empty) ? string.Empty : "\n\n",
				e.Message,
				e.StackTrace);

			if (e.InnerException != null)
				exceptionMessage = serializeException(e.InnerException, exceptionMessage);

			return exceptionMessage;
		}

		private static ILog getLogger(Type source)
		{
			if (!_loggers.ContainsKey(source))
			{
				lock (_loggers)
				{
					if (!_loggers.ContainsKey(source))
					{
						ILog logger = LogManager.GetLogger(source);
						_loggers.Add(source, logger);
					}
				}
			}

			return _loggers[source];
		}


		public void Info(object source, object message)
		{
			info(source, message);
		}

		public void Warn(object source, object message)
		{
			warn(source, message);
		}

		public void Error(object source, object message)
		{
			error(source, message);
		}

		public void Fatal(object source, object message)
		{
			fatal(source, message);
		}

		public void Debug(object source, object message)
		{
			debug(source, message);
		}

		public void SetInfo(Action<object, object> method)
		{
			info = method;
		}

		public void SetWarn(Action<object, object> method)
		{
			warn = method;
		}

		public void SetError(Action<object, object> method)
		{
			error = method;
		}

		public void SetFatal(Action<object, object> method)
		{
			fatal = method;
		}

		public void SetDebug(Action<object, object> method)
		{
			debug = method;
		}
	}
     */

    /*
         internal class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        #region Fields
        private readonly string _Name;
        private readonly ILog _Log;
        private static ILoggerRepository _LoggerRepository;
        #endregion

        public Log4NetLogger(string name, IAppender[] appenders)
        {
            _Name = name;

            if (_LoggerRepository != null)
                _Log = LogManager.GetLogger(_LoggerRepository.Name, name);

            if (_Log == null)
            {
                _LoggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                _Log = LogManager.GetLogger(_LoggerRepository.Name, name);
                log4net.Config.BasicConfigurator.Configure(_LoggerRepository, appenders);
            }
        }

        #region Public Methods
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _Log.IsDebugEnabled;
                case LogLevel.Error:
                    return _Log.IsErrorEnabled;
                case LogLevel.Information:
                    return _Log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            Log(logLevel, state, exception, formatter);
        }

        private void Log<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = null;

            if (null != formatter)
            {
                message = formatter(state, exception);
            }

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _Log.Fatal(message, exception);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _Log.Debug(message, exception);
                        break;
                    case LogLevel.Error:
                        _Log.Error(message, exception);
                        break;
                    case LogLevel.Information:
                        _Log.Info(message, exception);
                        break;
                    case LogLevel.Warning:
                        _Log.Warn(message, exception);
                        break;
                    default:
                        _Log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _Log.Info(message, exception);
                        break;
                }
            }
        }
        #endregion
    }
     */
}
