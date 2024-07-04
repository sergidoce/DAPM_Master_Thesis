using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry
{
    public class GetEntriesFromOrgResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public OrganizationDTO Organization { get; set; }
        public IEnumerable<RepositoryDTO> Repositories { get; set; }
        public IEnumerable<ResourceDTO> Resources { get; set; }
        public IEnumerable<PipelineDTO> Pipelines { get; set; }
    }
}
