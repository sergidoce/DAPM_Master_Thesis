using DAPM.ClientApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DAPM.ClientApi.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly ILogger<ActivityLogService> _logger;
        private readonly string _logFilePath;

        // Lock to prevent concurrent file access
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogService"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging information and errors.</param>
        public ActivityLogService(ILogger<ActivityLogService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "ActivityLog.txt");

            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
                _logger.LogInformation($"Created log directory at: {logDirectory}");
            }
        }

        public async Task LogUserActivity(string userName, string action, string result, DateTime timestamp, string clientIp)
        {
            try
            {
                // Format the log entry to include only Client IP
                var logEntry = $"{timestamp:yyyy-MM-dd HH:mm:ss} | User: {userName} | Action: {action} | Result: {result} | Client IP: {clientIp}{Environment.NewLine}";

                // Lock to prevent concurrent writes
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, logEntry);
                }

                // Log to the console for immediate visibility
                _logger.LogInformation($"Logged activity: {logEntry.Trim()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging activity: {ex.Message}");
            }

            await Task.CompletedTask;
        }

        public async Task<Stream> DownloadActivityLogAsync()
        {
            try
            {
                if (!File.Exists(_logFilePath))
                {
                    throw new FileNotFoundException("Activity log file not found.");
                }

                var memoryStream = new MemoryStream();
                await using (var fileStream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }

                memoryStream.Position = 0;
                _logger.LogInformation("Activity log file successfully prepared for download.");
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error preparing activity log for download: {ex.Message}");
                throw;
            }
        }
    }
}
