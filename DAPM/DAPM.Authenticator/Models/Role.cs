using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.Authenticator.Models
{
    public class Role : IdentityRole<int> 
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
