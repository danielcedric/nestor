//using log4net;
//using log4net.Core;
//using log4net.Repository;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Nestor.Tools.Logging
//{
//    public class Log4NetAdapter : log4net.Core.ILogger
//    {
//        #region T1
//        private ILog _logger;

//        public Log4NetAdapter(string loggerName)
//        {
//            _logger = LogManager.GetLogger(loggerName);
//        }

//        public string Name => throw new NotImplementedException();

//        public ILoggerRepository Repository => throw new NotImplementedException();

//        public IDisposable BeginScopeImpl(object state)
//        {
//            return null;
//        }

//        public bool IsEnabled(LogLevel logLevel)
//        {
//            switch (logLevel)
//            {
//                case LogLevel.Trace:
//                case LogLevel.Debug:
//                    return _logger.IsDebugEnabled;
//                case LogLevel.Information:
//                    return _logger.IsInfoEnabled;
//                case LogLevel.Warning:
//                    return _logger.IsWarnEnabled;
//                case LogLevel.Error:
//                    return _logger.IsErrorEnabled;
//                case LogLevel.Critical:
//                    return _logger.IsFatalEnabled;
//                default:
//                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
//            }
//        }

//        public bool IsEnabledFor(Level level)
//        {
//            throw new NotImplementedException();
//        }

//        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
//        {
//            if (!IsEnabled(logLevel))
//            {
//                return;
//            }
//            string message = null;
//            if (null != formatter)
//            {
//                message = formatter(state, exception);
//            }
//            else
//            {
//                message = LogFormatter.Formatter(state, exception);
//            }
//            switch (logLevel)
//            {
//                case LogLevel.Trace:
//                case LogLevel.Debug:
//                    _logger.Debug(message, exception);
//                    break;
//                case LogLevel.Information:
//                    _logger.Info(message, exception);
//                    break;
//                case LogLevel.Warning:
//                    _logger.Warn(message, exception);
//                    break;
//                case LogLevel.Error:
//                    _logger.Error(message, exception);
//                    break;
//                case LogLevel.Critical:
//                    _logger.Fatal(message, exception);
//                    break;
//                default:
//                    _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
//                    _logger.Info(message, exception);
//                    break;
//            }
//        }

//        public void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
//        {
//            throw new NotImplementedException();
//        }

//        public void Log(LoggingEvent logEvent)
//        {
//            throw new NotImplementedException();
//        }
//        #endregion

//    }
//}
