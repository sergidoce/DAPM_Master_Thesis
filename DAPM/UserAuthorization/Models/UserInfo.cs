using Newtonsoft.Json;

namespace UserAuthorization.Models
{
    public class UserInfo
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Status { get; set; } = 1;

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public DateTime Last_Login_Time { get; set; } = DateTime.Now;

        public string GroupID { get; set; } = string.Empty;

        public string OrganizationID { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public string permission_repositoryID { get; set; } = string.Empty;

        public string permission_repository_createID { get; set; } = string.Empty;

        public string permission_repository_readID { get; set; } = string.Empty;

        public string permission_resource_readID { get; set; } = string.Empty;

        public string permission_resource_downloadID { get; set; } = string.Empty;
    }
}
