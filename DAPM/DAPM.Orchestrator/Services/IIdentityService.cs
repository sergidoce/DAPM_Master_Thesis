using DAPM.Orchestrator.Services.Models;

namespace DAPM.Orchestrator.Services
{
    public interface IIdentityService
    {
        public Identity? GetIdentity();
        public Identity GenerateNewIdentity();
    }
}
