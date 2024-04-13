using DAPM.ClientApi.Services.Interfaces;
using System.Globalization;
using System.Resources;

namespace DAPM.ClientApi.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ILogger<ResourceService> _logger;
       public ResourceService(ILogger<ResourceService> logger)
       {
            _logger = logger;
       }

    }
}
