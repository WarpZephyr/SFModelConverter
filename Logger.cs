using System;
using System.IO;

namespace ACFAModelReplacer
{
    internal static class Logger
    {
        // Create log files, overwrite them if they already exist
        public static void createLog() 
        {
            File.WriteAllText(Util.acfaModelReplacerLog, String.Empty);
            File.WriteAllText(Util.stacktraceLog, String.Empty);
        }

        // Log when an exception has occurred with logs for the user and the dev
        public static void LogExceptionWithDate(Exception ex, string description) 
        {
            using (StreamWriter sw = File.AppendText(Util.acfaModelReplacerLog))
            {
                sw.WriteLine($"{description} on {DateTime.Now}");
            }

            using (StreamWriter sw = File.AppendText(Util.stacktraceLog))
            {
                sw.WriteLine($"Description: \"{description}\" on {DateTime.Now}\nException: {ex.Message}\nStacktrace: {ex}");
            }
        }

        // Log when no exception
        public static void LogWithDate(string description) 
        {
            using (StreamWriter sw = File.AppendText(Util.acfaModelReplacerLog))
            {
                sw.WriteLine($"{description} on {DateTime.Now}");
            }
        }

        // Log no date
        public static void Log(string description)
        {
            using (StreamWriter sw = File.AppendText(Util.acfaModelReplacerLog))
            {
                sw.WriteLine($"{description}");
            }
        }
    }
}
