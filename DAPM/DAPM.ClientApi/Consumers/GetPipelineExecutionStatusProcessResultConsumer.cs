using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetPipelineExecutionStatusProcessResultConsumer : IQueueConsumer<GetPipelineExecutionStatusRequestResult>
    {
        private ILogger<GetPipelineExecutionStatusProcessResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetPipelineExecutionStatusProcessResultConsumer(ILogger<GetPipelineExecutionStatusProcessResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetPipelineExecutionStatusRequestResult message)
        {
            _logger.LogInformation("GetPipelineExecutionStatusRequestResult received");


            var status = message.Status;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken statusJson = JToken.FromObject(status, serializer);
            result["status"] = statusJson;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
