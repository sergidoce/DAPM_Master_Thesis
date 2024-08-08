using Newtonsoft.Json;

namespace UserAuthorization.Models
{
    public class UserFile
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string FileId { get; set; } = string.Empty;

        public string OperaType { get; set; } = string.Empty;

        public int Status { get; set; } = 1;

        public DateTime CreateTime { get; set; } = DateTime.Now;

    }
}
