namespace UtilLibrary
{
    public class RegistrationDto
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public string OrganizationName { get; set; } = "";
        public Guid OrganizationId { get; set; }

        public List<string> Roles { get; set; }

    }
}
