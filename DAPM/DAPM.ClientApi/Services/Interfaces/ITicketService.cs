using Newtonsoft.Json.Linq;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface ITicketService
    {
        public TicketStatus GetTicketStatus(Guid ticketId);
        public JToken GetTicketResolution(Guid ticketId);
        public Guid CreateNewTicket();
        public void UpdateTicketResolution(Guid ticketId, JToken resolution);
    }
}
