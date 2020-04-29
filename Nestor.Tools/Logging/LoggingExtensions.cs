//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Nestor.Tools.Logging
//{
//    public static class LoggingExtensions
//    {
//        /// <summary>
//        /// Append a logging provider to provide an output into a log file using log4Net library
//        /// </summary>
//        /// <param name="factory"></param>
//        /// <returns></returns>
//        public static ILoggerFactory UseLog4Net(this ILoggerFactory factory)
//        {
//            factory.AddProvider(new Log4NetProvider());
//            return factory;
//        }

//        /// <summary>
//        /// Append a logging provider to provide an output in the console
//        /// </summary>
//        /// <param name="factory"></param>
//        /// <returns></returns>
//        public static ILoggerFactory UseConsole(this ILoggerFactory factory)
//        {

//            return factory;
//        }
//    }
//}
