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
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationController(
            IMapper mapper,
            IConfiguration configuration,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
        {
            User user = _mapper.Map<User>(registerDto);

            if (!UserExists(registerDto.UserName))
            {
                var registerUserResult = await _userManager.CreateAsync(user, registerDto.Password);
                if (!registerUserResult.Succeeded) return BadRequest(registerUserResult.Errors);

                var addRoleResult = await _userManager.AddToRoleAsync(user, "Standard");
                if (!addRoleResult.Succeeded) return BadRequest(addRoleResult.Errors);
            }
            else
            {
                return BadRequest("Username is already in use");
            }

            return Ok("okily dokily");

        }

        private bool UserExists(string name) {
            User result = _userManager.FindByNameAsync(name).GetAwaiter().GetResult();
            if (result == null) { 
                return false;
            }
            return true;
        } 
    }
}
