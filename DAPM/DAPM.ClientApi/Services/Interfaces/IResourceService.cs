using Microsoft.AspNetCore.Mvc;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IResourceService
    {
        public Guid GetResourceById(Guid organizationId, Guid repositoryId, Guid resourceId);
        public Guid GetResourceFileById(Guid organizationId, Guid repositoryId, Guid resourceId);
    }
}
