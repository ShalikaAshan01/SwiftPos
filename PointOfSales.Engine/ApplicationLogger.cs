using Microsoft.Extensions.Logging;
using PointOfSales.Core.Constants;
using System.Text;
using PointOfSales.Core.Utils;

namespace PointOfSales.Engine
{
    public class ApplicationLogger : IApplicationLogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new();

        public ApplicationLogger()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            _logFilePath = Path.Combine(LocalConfigurations.LocalFolderPath, "Logs");
            if (!Directory.Exists(_logFilePath))
            {
                Directory.CreateDirectory(_logFilePath);
            }
            _logFilePath = Path.Combine(_logFilePath, $"{today}.log");
        }

        public void LogInfo(string message, params object[] args)
        {
            WriteLog("INFO", message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            WriteLog("WARN", message, args);
        }

        public void LogError(string message, params object[] args)
        {
            WriteLog("ERROR", message, args);
        }

        public void LogError(Exception exception, string message, params object[]? args)
        {
            var errorMessage = args is { Length: > 0 }
                ? string.Format(message, args)
                : message;

            var fullMessage = new StringBuilder();
            fullMessage.AppendLine(errorMessage);
            fullMessage.AppendLine("Exception:");
            fullMessage.AppendLine(exception.ToString());

            WriteRawLog("ERROR", fullMessage.ToString());
        }
        
        private void WriteRawLog(string level, string fullMessage)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logEntry = $"[{timestamp}] [{level}] {fullMessage}";

#if DEBUG
            Console.WriteLine(logEntry);
#endif

            lock (_lock)
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
            }
        }

        private void WriteLog(string level, string message, params object[] args)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string formattedMessage = args.Length > 0
                ? string.Format(message, args)
                : message;

            string logLine = $"[{timestamp}] [{level}] {formattedMessage}";
            #if DEBUG
            Console.WriteLine(logLine);
            #endif
            lock (_lock)
            {
                File.AppendAllText(_logFilePath, logLine + Environment.NewLine, Encoding.UTF8);
            }
        }
    }


}
