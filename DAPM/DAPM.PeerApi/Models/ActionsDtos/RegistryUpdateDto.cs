using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.ActionsDtos
{
    public class RegistryUpdateDto
    {
        public IdentityDTO SenderIdentity { get; set; }
        public Guid? HandshakeId { get; set; }
        public bool IsPartOfHandshake { get; set; }
        public IEnumerable<OrganizationDTO> Organizations { get; set; }
        public IEnumerable<RepositoryDTO> Repositories { get; set; }
        public IEnumerable<ResourceDTO> Resources { get; set; }
        public IEnumerable<PipelineDTO> Pipelines { get; set; }
    }
}
