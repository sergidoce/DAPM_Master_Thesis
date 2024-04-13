using DAPM.ClientApi.Services.Interfaces;

namespace DAPM.ClientApi.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ILogger<RepositoryService> _logger;

        public RepositoryService(ILogger<RepositoryService> logger) 
        {
            _logger = logger;
        }
    }
}
