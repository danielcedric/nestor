//using Microsoft.Extensions.Logging;
//using log4net;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Nestor.Tools.Logging
//{
//    public class Log4NetProvider : ILoggerProvider
//    {
//        private IDictionary<string, ILogger> loggers
//            = new Dictionary<string, ILogger>();

//        public Log4NetProvider()
//        {

//        }

//        public ILogger CreateLogger(string name)
//        {
//            if (!loggers.ContainsKey(name))
//            {
//                lock (loggers)
//                {
//                    // Have to check again since another thread may have gotten the lock first
//                    if (!loggers.ContainsKey(name))
//                    {
//                        loggers[name] = new Log4NetAdapter(name);
//                    }
//                }
//            }
//            return loggers[name];
//        }

//        public void Dispose()
//        {
//            loggers.Clear();
//            loggers = null;
//        }
//    }


//}

