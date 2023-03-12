using System;
using System.IO;

namespace SFModelConverter
{
    internal static class Logger
    {
        public static string log = $"{Util.envFolderPath}/modelreplacer.log";
        public static string stacktraceLog = $"{Util.envFolderPath}/stacktrace.log";

        /// <summary>
        /// Create log files and overwrite them if they already exist.
        /// </summary>
        public static void CreateLog()
        {
            File.WriteAllText(log, String.Empty);
            File.WriteAllText(stacktraceLog, String.Empty);
        }

        /// <summary>
        /// Log something with an exception, date, and time.
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="description">The description of what to log</param>
        public static void LogExceptionWithDate(Exception ex, string description = null)
        {
            using StreamWriter swLog = File.AppendText(log);
            swLog.WriteLine($"{description} on {DateTime.Now}");
            swLog.Close();

            using StreamWriter swStacktrace = File.AppendText(stacktraceLog);
            swStacktrace.WriteLine($"Description: \"{description ?? "Unknown Error"}\" on {DateTime.Now}\nException: {ex.Message}\nStacktrace: {ex}");
            swStacktrace.Close();
        }

        /// <summary>
        /// Log something with the date and time.
        /// </summary>
        /// <param name="description">The description of what to log</param>
        public static void LogWithDate(string description = null)
        {
            using StreamWriter sw = File.AppendText(log);
            sw.WriteLine($"{description ?? "Log with date was called"} on {DateTime.Now}");
            sw.Close();
        }

        /// <summary>
        /// Log something.
        /// </summary>
        /// <param name="description">The description of what to log</param>
        public static void Log(string description = null)
        {
            using StreamWriter sw = File.AppendText(log);
            sw.WriteLine($"{description ?? "Log was called"}");
            sw.Close();
        }
    }
}
