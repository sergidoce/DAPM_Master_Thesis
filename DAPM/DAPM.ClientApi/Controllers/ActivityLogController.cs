using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        private readonly ILogger<ActivityLogController> _logger;

        public ActivityLogController(IActivityLogService activityLogService, ILogger<ActivityLogController> logger)
        {
            _activityLogService = activityLogService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getLogs")]
        public IActionResult GetActivityLogs()
        {
            try
            {
                var logs = System.IO.File.ReadAllLines("Logs/ActivityLog.txt");
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error reading logs: {ex.Message}");
                return StatusCode(500, $"Error reading logs: {ex.Message}");
            }
        }

        [HttpGet("downloadLogs")]
        public async Task<IActionResult> DownloadActivityLogs()
        {

            try
            {
                // Call the async method using the injected service
                var logStream = await _activityLogService.DownloadActivityLogAsync();

                return File(logStream, "text/plain", "ActivityLog.txt");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Error downloading activity log: {ex.Message}");
                return NotFound("Log file not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
