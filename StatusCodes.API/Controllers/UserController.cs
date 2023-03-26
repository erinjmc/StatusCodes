using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;
using StatusCodes.API.Services;

namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public UserController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }


        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            var codes = _statusRepository.GetUsers();
            if (codes == null)
            {
                return BadRequest("Record not found!");
            }
            return Ok(codes);
        }

        [HttpGet("user")]
        public ActionResult GetUser(string username)
        {
            var user = _statusRepository.GetUser(username);
            if (user == null)
            {
                return BadRequest("Record not found!");
            }
            return Ok(user);
        }

        [HttpPost("user/new")]
        public ActionResult NewUser(string firstname, string lastname, string email, bool isadmin, string password)
        {
            if(_statusRepository.ValidateUser(new AuthRequest { UserName = email, Password = password }) == String.Empty)
            {
                var newuser = _statusRepository.NewUser(new User { FirstName = firstname, LastName = lastname, Email = email.ToLower(), IsAdmin = isadmin }, password);
                return Ok(newuser);

            }
            return BadRequest("A user with that email already exist!");
        }

        [HttpPut("user")]
        public ActionResult UpdateUser(User user) 
        {
            var newuser = _statusRepository.UpdateUser(user);
            return Ok();
        }

        [HttpDelete("user")]
        public ActionResult DeleteUser(string username)
        {
            if(_statusRepository.DeleterUser(username))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
