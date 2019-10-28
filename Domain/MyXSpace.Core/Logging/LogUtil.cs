using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace MyXSpace.Core.Logging
{
    public static class LoggerUtil
    {
        static ILoggerFactory _loggerFactory;

        /// <summary>
        /// Confire logger once (in top module = web app)
        /// Then CreateLogger<T> can be reused in other places
        /// </summary>
        /// <param name="loggerFactory"></param>
        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
           
            //TODO: log to file or DB          
            //var file = Path.Combine(Directory.GetCurrentDirectory(), "logger.txt");
            //loggerFactory.AddFile(file);
            //var fileLogger = loggerFactory.CreateLogger("FileLogger");
            
        }

        /// <summary>
        /// Create logger for T type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ILogger<T> CreateLogger<T>()
        {
            if (_loggerFactory == null)
            {
                //throw new InvalidOperationException($"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");
                _loggerFactory = new LoggerFactory();
            }

            return _loggerFactory.CreateLogger<T>();
        }

    }

}
