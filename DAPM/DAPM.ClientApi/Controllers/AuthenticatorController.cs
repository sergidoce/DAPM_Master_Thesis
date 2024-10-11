using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPM.ClientApi.Controllers
{
    
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("auth")]
    public class AuthenticatorController : ControllerBase
    {
        private readonly ILogger<AuthenticatorController> _logger;
        //private IAuthenticatorService _authenticationService;

        public AuthenticatorController(ILogger<AuthenticatorController> logger/*, IAuthenticatorService authenticationService*/)
        {
            _logger = logger;
            //_authenticationService = authenticationService;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<Guid>> SignUp()
        {
            return Ok();
        }

        [HttpGet("log-in")]
        public async Task<ActionResult<Guid>> LogIn()
        {
            return Ok();
        }

        [HttpPost("add-user")]
        public async Task<ActionResult<Guid>> AddUser()
        {
            return Ok();
        }

        
        [HttpDelete("remove-user")]
        public async Task<ActionResult<Guid>> RemoveUser()
        {
            return Ok();
        }
    }
}
