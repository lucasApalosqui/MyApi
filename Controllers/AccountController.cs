using BlogAspNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {

        [HttpPost("v1/login")]
        public IActionResult login([FromServices] TokenService tokenService)
        {
            var token = tokenService.GenerateToken(null);

            return Ok(token);
        }

        [Authorize(Roles = "user")]
        [HttpGet("v1/user")]
        public IActionResult GetUser()
        {
            return Ok(User.Identity.Name);
        }


        [Authorize(Roles = "author")]
        [HttpGet("v1/author")]
        public IActionResult GetAuthor()
        {
            return Ok(User.Identity.Name);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("v1/admin")]
        public IActionResult GetAdmin()
        {
            return Ok(User.Identity.Name);
        }

    }
}
