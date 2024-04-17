using Newtonsoft.Json.Linq;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface ITicketService
    {
        public int GetTicketStatus(Guid ticketId);
        public JObject GetTicketResolution(Guid ticketId);
        public Guid CreateNewTicket();
        public void UpdateTicketResolution(Guid ticketId, JObject resolution);
    }
}
