using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace DAPM.ClientApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly ILogger<ITicketService> _logger;
        private Dictionary<Guid, JObject> _ticketResolutions;
        private Dictionary<Guid, int> _ticketStatus;

        public TicketService(ILogger<ITicketService> logger) 
        {
            _logger = logger;
            _ticketStatus = new Dictionary<Guid, int>();
            _ticketResolutions = new Dictionary<Guid, JObject>();
        }
        public JObject GetTicketResolution(Guid ticketId)
        {
            return _ticketResolutions[ticketId];
        }

        public int GetTicketStatus(Guid ticketId)
        {
            return _ticketStatus[ticketId];
        }

        public Guid CreateNewTicket()
        {
            Guid guid = Guid.NewGuid();
            _ticketStatus[guid] = 0;
            _logger.LogInformation($"A new ticket has been created");
            return guid;
        }

        public void UpdateTicketResolution(Guid ticketId, JObject resolution)
        {
            _ticketResolutions[ticketId] = resolution;
            _ticketStatus[ticketId] = 1;
            _logger.LogInformation($"Ticket resolution of ticket {ticketId} has been updated");
        }
    }
}
