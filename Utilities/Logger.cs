using System;
using System.IO;

namespace Utilities
{
    internal static class Logger
    {
        /// <summary>
        /// Create log files and overwrites them if they already exist.
        /// </summary>
        /// <param name="logName">The name to give to the log file to be created.</param>
        /// <param name="stackTraceLogName">The name to give to the stacktrace log file to be created.</param>
        public static void CreateLog(string logName = null, string stackTraceLogName = null) 
        {
            File.WriteAllText(logName ?? PathUtil.Log, string.Empty);
            File.WriteAllText(stackTraceLogName ?? PathUtil.StackTraceLog, string.Empty);
        }

        /// <summary>
        /// Log something with an exception, date, and time.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="description">The description of what to log.</param>
        public static void LogExceptionWithDate(Exception ex, string description = null) 
        {
            using (StreamWriter swLog = File.AppendText(PathUtil.Log))
            {
                swLog.WriteLine($"{description} on {DateTime.Now}");
            }

            using (StreamWriter swStacktrace = File.AppendText(PathUtil.StackTraceLog))
            {
                swStacktrace.WriteLine($"Description: \"{description ?? "Unknown Error"}\" on {DateTime.Now}\nException: {ex.Message}\nStacktrace: {ex}");
                swStacktrace.Close();
            }
        }

        /// <summary>
        /// Log something with an exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="description">The description of what to log.</param>
        public static void LogException(Exception ex, string description = null)
        {
            using (StreamWriter swLog = File.AppendText(PathUtil.Log))
            {
                swLog.WriteLine($"{description}");
            }

            using (StreamWriter swStacktrace = File.AppendText(PathUtil.StackTraceLog))
            {
                swStacktrace.WriteLine($"Description: \"{description ?? "Unknown Error"}\" \nException: {ex.Message}\nStacktrace: {ex}");
            }
        }

        /// <summary>
        /// Log something with the date and time.
        /// </summary>
        /// <param name="description">The description of what to log</param>
        public static void LogWithDate(string description = null) 
        {
            using (StreamWriter sw = File.AppendText(PathUtil.Log))
            {
                sw.WriteLine($"{description ?? "Log with date was called"} on {DateTime.Now}");
            }
        }

        /// <summary>
        /// Log something.
        /// </summary>
        /// <param name="description">The description of what to log.</param>
        public static void Log(string description = null)
        {
            using (StreamWriter sw = File.AppendText(PathUtil.Log))
            {
                sw.WriteLine($"{description ?? "Log was called"}");
            }
        }
    }
}
