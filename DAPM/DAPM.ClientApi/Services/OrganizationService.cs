using DAPM.ClientApi.Services.Interfaces;

namespace DAPM.ClientApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;

        public OrganizationService(ILogger<OrganizationService> logger) 
        {
            _logger = logger;
        }
    }
}
