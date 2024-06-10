using DAPM.Orchestrator.Services.Models;
using System.Text.Json;

namespace DAPM.Orchestrator.Services
{
    public class IdentityService : IIdentityService
    {
        private ILogger<IdentityService> _logger;
        private string _identityConfigurationPath;

        public IdentityService(ILogger<IdentityService> logger)
        {
            _logger = logger;

            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Services");
            path = Path.Combine(path, "Configuration");
            _identityConfigurationPath = Path.Combine(path, "IdentityConfiguration.json");
        }

        public Identity GenerateNewIdentity()
        {

            Identity identity = ReadCurrentIdentity();

            identity.Id = Guid.NewGuid();
            
            string jsonString = JsonSerializer.Serialize(identity);
            File.WriteAllText(_identityConfigurationPath, jsonString);

            return identity;
        }

        public Identity? GetIdentity()
        {
            Identity identity = ReadCurrentIdentity();

            if(identity.Id == null || !identity.Id.HasValue)
            {
                return GenerateNewIdentity();
            }
            else
            {
                return identity;
            }
        }


        private Identity ReadCurrentIdentity()
        {
            string jsonString = File.ReadAllText(_identityConfigurationPath);
            return JsonSerializer.Deserialize<Identity>(jsonString)!;
        }
    }
}
