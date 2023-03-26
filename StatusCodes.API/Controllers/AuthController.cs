using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;

namespace StatusCodes.API.Controllers
{
    [Route("api/logon")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IStatusRepository statusRepository;

        public AuthController(IStatusRepository _statusRepository)
        {
            statusRepository = _statusRepository;
        }
      
        [HttpPost]
        public ActionResult Logon(string username, string password)
        {
            var result = statusRepository.ValidateUser(new AuthRequest { UserName = username.ToLower(), Password = password });
            if (result == String.Empty)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}
