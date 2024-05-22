using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetRepositoriesResultConsumer : IQueueConsumer<GetRepositoriesResultMessage>
    {
        private ILogger<GetRepositoriesResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetRepositoriesResultConsumer(ILogger<GetRepositoriesResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetRepositoriesResultMessage message)
        {
            _logger.LogInformation("GetRepositoriesResultMessage received");


            IEnumerable<RepositoryDTO> repositoriesDTOs = message.Repositories;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken repositoriesJSON = JToken.FromObject(repositoriesDTOs, serializer);
            result["repositories"] = repositoriesJSON;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
