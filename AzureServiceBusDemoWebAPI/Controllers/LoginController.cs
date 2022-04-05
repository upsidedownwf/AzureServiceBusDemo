using System.Threading.Tasks;
using AzureServiceBusDemoWebAPI.Models;
using AzureServiceBusDemoWebAPI.Models.Attributes;
using AzureServiceBusDemoWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthorizeAttribute = AzureServiceBusDemoWebAPI.Models.Attributes.AuthorizeAttribute;

namespace AzureServiceBusDemoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ITokenProvider _tokenProvider;

        public LoginController(ILogger<LoginController> logger, ITokenProvider tokenProvider)
        {
            _logger = logger;
            _tokenProvider = tokenProvider;
        }
        [HttpPost]
        public IActionResult Login(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string token = _tokenProvider.LoginUser(userModel.USERID, userModel.PASSWORD);
            Request.HttpContext.Session.SetString("JWToken", token);
            return Ok(token);
        }
        [HttpGet, Authenticate]
        public IActionResult GetUsers()
        {
            var users = _tokenProvider.userList;
            return Ok(users);
        }
        [HttpGet("{id}"), Authorize(Roles.DIRECTOR, Roles.SUPERVISOR)]
        public IActionResult GetUser(string id)
        {
            return Ok("Authorized");
        }
    }
}
