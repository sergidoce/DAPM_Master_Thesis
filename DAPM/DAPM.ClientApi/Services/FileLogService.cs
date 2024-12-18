using System;
using System.IO;
using System.Threading.Tasks;

namespace DAPM.ClientApi.Services
{
    public class FileLogService
    {
        private readonly string _logFilePath;

        public FileLogService(string logFilePath)
        {
            _logFilePath = logFilePath;

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public async Task LogActivityAsync(string userName, string action, string result, DateTime timestamp)
        {
            string logEntry = $"{timestamp:yyyy-MM-dd HH:mm:ss} | User: {userName} | Action: {action} | Result: {result}";

            // Write to file
            await File.AppendAllTextAsync(_logFilePath, logEntry + Environment.NewLine);
        }
    }
}
