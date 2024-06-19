using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary.Models
{
    public class TransferDataActionDTO
    {
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }

        public Guid OriginOrganizationId { get; set; }
        public Guid? OriginRepositoryId { get; set; }
        public Guid OriginResourceId { get; set; }

        public int SourceStorageMode { get; set; }
        public int DestinationStorageMode { get; set; }

        public Guid DestinationOrganizationId { get; set; }
        public Guid? DestinationRepositoryId { get; set; }
    }
}
