namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IOrganizationService
    {
        public Guid GetOrganizations();
        public Guid GetOrganizationById(int organizationId);
        public Guid GetUsersOfOrganization(int organizationId); 
        public Guid GetRepositoriesOfOrganization(int organizationId);
    }
}
