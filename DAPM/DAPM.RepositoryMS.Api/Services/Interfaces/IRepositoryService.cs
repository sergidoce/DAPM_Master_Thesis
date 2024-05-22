namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IRepositoryService
    {
        Task<int> CreateNewResource(int repositoryId, string name, byte[] resourceFile);
    }
}
