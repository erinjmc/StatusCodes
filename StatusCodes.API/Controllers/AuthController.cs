using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;
using StatusCodes.API.Services;
using System.Security.Claims;

namespace StatusCodes.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public AuthController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }
      
        [HttpPost("logon")]
        public ActionResult Logon(string username, string password)
        {
            var result = _statusRepository.ValidateUser(new AuthRequest { UserName = username.ToLower(), Password = password });
            if (result == String.Empty)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        [HttpPost("logout")] 
        public ActionResult Logout()
        {
            var claims = User.Claims.ToList();
            
            var result = _statusRepository.InvalidateUser(claims);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
