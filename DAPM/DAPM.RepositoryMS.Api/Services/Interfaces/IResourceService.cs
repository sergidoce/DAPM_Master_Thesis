using DAPM.RepositoryMS.Api.Models;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<bool> PublishResource(Resource resource);

        Task<Resource> RetrieveResource(string resourceName);
    }
}
