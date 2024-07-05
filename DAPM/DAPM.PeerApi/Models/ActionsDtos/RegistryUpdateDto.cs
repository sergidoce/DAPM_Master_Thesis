using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.ActionsDtos
{
    public class RegistryUpdateDto
    {
        public Guid SenderProcessId { get; set; }
        public IdentityDTO SenderIdentity { get; set; }
        public Guid RegistryUpdateId { get; set; }
        public bool IsPartOfHandshake { get; set; }
        public IEnumerable<OrganizationDTO> Organizations { get; set; }
        public IEnumerable<RepositoryDTO> Repositories { get; set; }
        public IEnumerable<ResourceDTO> Resources { get; set; }
        public IEnumerable<PipelineDTO> Pipelines { get; set; }
    }
}
