namespace DAPM.ClientApi.Models
{
    public class ActivityLog
    {
        public int Id { get; set; } 
        public string UserName { get; set; } 
        public string Action { get; set; }
        public string Result { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
