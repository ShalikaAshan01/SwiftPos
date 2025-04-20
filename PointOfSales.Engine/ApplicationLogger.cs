using Microsoft.Extensions.Logging;
using PointOfSales.Core.Constants;
using System.Text;

namespace PointOfSales.Engine
{
    public interface IApplicationLogger
    {
        public void LogInfo(string message,params object[] args);
        public void LogWarning(string message, params object[] args);
        public void LogError(string message, params object[] args);
    }
    public class ApplicationLogger : IApplicationLogger
    {
        private readonly string _logFilePath;
        private readonly object _lock = new();

        public ApplicationLogger()
        {
            _logFilePath = Path.Combine(LocalConfigurations.LocalFolderPath, LocalConfigurations.LogFileName);

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

        private void WriteLog(string level, string message, params object[] args)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string formattedMessage = args != null && args.Length > 0
                ? string.Format(message, args)
                : message;

            string logLine = $"[{timestamp}] [{level}] {formattedMessage}";

            lock (_lock)
            {
                File.AppendAllText(_logFilePath, logLine + Environment.NewLine, Encoding.UTF8);
            }
        }
    }


}
