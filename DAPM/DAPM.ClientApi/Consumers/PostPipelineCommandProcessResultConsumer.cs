using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Interfaces;

namespace DAPM.ClientApi.Consumers
{
    public class PostPipelineCommandProcessResultConsumer : IQueueConsumer<PostPipelineCommandProcessResult>
    {
        private ILogger<PostPipelineCommandProcessResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public PostPipelineCommandProcessResultConsumer(ILogger<PostPipelineCommandProcessResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(PostPipelineCommandProcessResult message)
        {
            _logger.LogInformation("PostPipelineCommandProcessResult received");


            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            //Serialization
            result["executionId"] = message.ExecutionId;
            result["succeeded"] = message.Succeeded;
            result["message"] = message.Message;

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
