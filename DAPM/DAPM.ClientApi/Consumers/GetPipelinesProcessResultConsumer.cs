using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetPipelinesProcessResultConsumer : IQueueConsumer<GetPipelinesProcessResult>
    {
        private ILogger<GetPipelinesProcessResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetPipelinesProcessResultConsumer(ILogger<GetPipelinesProcessResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetPipelinesProcessResult message)
        {
            _logger.LogInformation("GetPipelinesProcessResult received");


            IEnumerable<PipelineDTO> pipelinesDTOs = message.Pipelines;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken pipelinesJSON = JToken.FromObject(pipelinesDTOs, serializer);
            result["pipelines"] = pipelinesJSON;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
