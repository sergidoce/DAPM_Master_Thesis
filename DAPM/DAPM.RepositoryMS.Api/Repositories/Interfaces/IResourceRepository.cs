using DAPM.RepositoryMS.Api.Models;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<bool> AddResource(Resource resource);
        Task<Resource> RetrieveResource(string resourceName);
    }
}
