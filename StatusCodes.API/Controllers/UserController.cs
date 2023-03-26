using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;


namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IStatusRepository statusRepository;

        public UserController(IStatusRepository _statusRepository)
        {
            statusRepository = _statusRepository;
        }


        [HttpGet]
        public ActionResult GetUsers()
        {
            var codes = statusRepository.GetUsers();
            return Ok(codes);
        }

        [HttpGet("{username}")]
        public ActionResult GetUser(string username)
        {
            var codes = statusRepository.GetUser(username);
            if (codes == null)
            {
                return BadRequest("Record not found!");
            }
            return Ok(codes);
        }

        [HttpPost("new")]
        public ActionResult NewUser(string firstname, string lastname, string email, bool isadmin, string password)
        {
            if(statusRepository.ValidateUser(new AuthRequest { UserName = email, Password = password }) == String.Empty)
            {
                var newuser = statusRepository.NewUser(new User { FirstName = firstname, LastName = lastname, Email = email.ToLower(), IsAdmin = isadmin }, password);
                return Ok(newuser);

            }
            return BadRequest("A user with that email already exist!");
        }
    }
}
