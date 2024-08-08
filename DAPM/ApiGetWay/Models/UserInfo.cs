using Newtonsoft.Json;

namespace ApiGetWay.Models
{
    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        [JsonProperty(Required = Required.Default)]
        public int Status { get; set; } = 1;

        [JsonProperty(Required = Required.Default)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [JsonProperty(Required = Required.Default)]
        public DateTime Last_Login_Time { get; set; } = DateTime.Now;

        [JsonProperty(Required = Required.Default)]
        public string OrganizationID { get; set; } = string.Empty;

        [JsonProperty(Required = Required.Default)]
        public string GroupID { get; set; } = string.Empty;

        [JsonProperty(Required = Required.Default)]
        public string RoleName { get; set; } = string.Empty;

        public string permission_repositoryID { get; set; } = string.Empty;

        public string permission_repository_createID { get; set; } = string.Empty;

        public string permission_repository_readID { get; set; } = string.Empty;

        public string permission_resource_readID { get; set; } = string.Empty;

        public string permission_resource_downloadID { get; set; } = string.Empty;
    }
}
