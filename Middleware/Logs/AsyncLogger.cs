using log4net;
using System.Collections.Concurrent;

namespace LibrarySystem.Middleware.Logs
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
    public static class AsyncLogger
    {
        private static readonly BlockingCollection<LogItem> logQueue = new();
        //private static readonly ILog logger = LogManager.GetLogger(typeof(AsyncLogger));
        private static readonly Task worker;

        static AsyncLogger()
        {
            
            worker = Task.Factory.StartNew(
                ProcessQueue,
                TaskCreationOptions.LongRunning);
        }
        public static void Log(LogLevel level, string message, Exception? ex = null, Type? callerType = null)
        {
            logQueue.Add(new LogItem
            {
                Level = level,
                Message = message,
                Exception = ex,
                LoggerName = callerType?.FullName ?? "DefaultLogger"
            });
        }
        private static void ProcessQueue()
        {
            foreach (var item in logQueue.GetConsumingEnumerable())
            {
                try
                {
                    var logger = LogManager.GetLogger(item.LoggerName);
                    switch (item.Level)
                    {
                        case LogLevel.Debug: logger.Debug(item.Message, item.Exception); break;
                        case LogLevel.Info: logger.Info(item.Message, item.Exception); break;
                        case LogLevel.Warn: logger.Warn(item.Message, item.Exception); break;
                        case LogLevel.Error: logger.Error(item.Message, item.Exception); break;
                        case LogLevel.Fatal: logger.Fatal(item.Message, item.Exception); break;
                    }
                }
                catch
                {
                   
                }
            }
        }
        private class LogItem
        {
            public LogLevel Level { get; set; }
            public string Message { get; set; } = string.Empty;
            public Exception? Exception { get; set; }
            public string LoggerName { get; set; }

        }

        public static void Shutdown()
        {
            logQueue.CompleteAdding(); 
            worker.Wait();            
        }
    }
}   
