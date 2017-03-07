using Announcer.Interfaces;
using System;
using System.Diagnostics;

namespace Announcer
{
    public class Logger
    {
        public enum Severity { debug, info, error }

        public static ILogWriter LogWriter;

        public static void Log(string message, Severity severity)
        {
            if (severity == Severity.error)
                Console.WriteLine("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] (ERROR) " + message);
            else if (severity == Severity.info)
                Console.WriteLine("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] " + message);
            else if (severity == Severity.debug)
                Console.WriteLine("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] (DEBUG) " + message);
#if DEBUG
            Debug.WriteLine("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] " + message);
#endif

            if (LogWriter != null)
            {
                LogWriter.WriteLogFile("[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] " + message, severity);
            }
        }
    }
}
