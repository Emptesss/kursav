using System;
using System.IO;
using System.Diagnostics;

namespace EducationalEventGenerator
{
    public static class Logger
    {
        private static readonly string LogPath = "game_log.txt";

        public static void Log(string message)
        {
            string logMessage = $"[{DateTime.Now}] {message}";
            Debug.WriteLine(logMessage);
            File.AppendAllText(LogPath, logMessage + Environment.NewLine);
        }
    }
}
