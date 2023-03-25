using Microsoft.AspNetCore.Mvc;

namespace StatusCodes.API.Controllers
{
    [Route("api/logon")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public class AuthRequestBody 
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("logon")]
        public ActionResult<string> Logon(AuthRequestBody _authRequestBody)
        {
            var result = ValidateCredentials(_authRequestBody.UserName, _authRequestBody.Password);
            if (!result)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        private bool ValidateCredentials(string? username, string? password)
        {
            return true;
        }
    }
}
