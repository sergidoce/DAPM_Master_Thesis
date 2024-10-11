using Microsoft.AspNetCore.Mvc;

namespace DAPM.Authenticator.Models.Dto
{
    public class OrganisationsDto 
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; } = "No affiliation";

    }
}
