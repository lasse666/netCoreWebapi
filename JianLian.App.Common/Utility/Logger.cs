using log4net;
using log4net.Config;
using System;

namespace Gym.App.Common
{
    public class Logger
    {
        private static ILog _loger;
        static Logger()
        {
          //  XmlConfigurator.Configure(); TODO.
            _loger = LogManager.GetLogger(typeof(Logger));
        }

        /// <summary>
        /// Log4日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error( Exception ex,string msg = "出现异常")
        {
            _loger.Error(msg, ex);
        }

        /// <summary>
        /// Log4日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg)
        {
            _loger.Warn(msg);
        }

        /// <summary>
        /// Log4日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            _loger.Info(msg);
        }

        /// <summary>
        /// Log4日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            _loger.Debug(msg);
        }
    }
}
