using System;
using System.IO;
using System.Threading.Tasks;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IActivityLogService
    {
        //Task LogUserActivity(string userName, string action, string result, DateTime timestamp);
        Task LogUserActivity(string userName, string action, string result, DateTime timestamp, string clientIp);

        Task<Stream> DownloadActivityLogAsync(); // Added method for downloading activity logs
    }
}
