using DAPM.ClientApi.Models;
using System.Xml.Linq;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IOrganizationService
    {
        public Guid GetOrganizations();
        public Guid GetOrganizationById(int organizationId);
        public Guid GetRepositoriesOfOrganization(int organizationId);
        public Guid PostRepositoryToOrganization(int organizationId, string name);
    }
}
