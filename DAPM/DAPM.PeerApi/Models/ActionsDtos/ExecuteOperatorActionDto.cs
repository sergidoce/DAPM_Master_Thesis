﻿using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Models.ActionsDtos
{
    public class ExecuteOperatorActionDto
    {
        public IdentityDTO SenderIdentity { get; set; }
        public Guid ExecutionId { get; set; }
        public Guid StepId { get; set; }
        public ExecuteOperatorActionDTO Data { get; set; }
    }
}