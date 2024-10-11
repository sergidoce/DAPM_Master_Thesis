using DAPM.Authenticator.Models.Dto;
using AutoMapper;
using DAPM.Authenticator.Models;
using DAPM.Authenticator.Models.Dto;
using DAPM.Authenticator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DAPM.Authenticator.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private RoleManager<Role> _rolemanager;

        public UserManagementController(RoleManager<Role> roleManager)
        {
            _rolemanager = roleManager;
        }

        [HttpPost("setOrg")]
        public async Task<IActionResult> SetOrganizationOfUser([FromBody] OrganisationsDto organisationsDto) {
            return Ok("not implemented");
        }


        [HttpPost("setRoles/{userid}")]
        public async Task<IActionResult> SetRoles([FromBody] List<string> listofroles, int userid)
        {
            if (listofroles == null)
            {
                return BadRequest("payload empty");
            }

            foreach (string role in listofroles) {
                Role result = await _rolemanager.FindByNameAsync(role);
                if (result == null)
                {
                    return BadRequest("Attempting to add non existent role");

                }              
            }



            return Ok("not implemented");
        }
    }
}
