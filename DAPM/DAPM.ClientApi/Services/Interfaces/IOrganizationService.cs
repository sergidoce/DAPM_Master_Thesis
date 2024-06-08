using DAPM.ClientApi.Models;
using System.Xml.Linq;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IOrganizationService
    {
        public Guid GetOrganizations();
        public Guid GetOrganizationById(Guid organizationId);
        public Guid GetRepositoriesOfOrganization(Guid organizationId);
        public Guid PostRepositoryToOrganization(Guid organizationId, string name);
    }
}
