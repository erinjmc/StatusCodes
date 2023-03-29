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


        [HttpGet("get/users")]
        public ActionResult GetUsers()
        {
            var response = _statusRepository.GetUsers();
            if (response.IsSuccess)
            {
                return Ok(response); 
            }
            return NotFound(response);
        }

        [HttpGet("get/user")]
        public ActionResult GetUser(int id)
        {
            var response = _statusRepository.GetUser(id);
            if (response == null)
            {
                return Ok(response);
            }
            return NotFound(response);
            
        }

        [HttpPost("new/user")]
        public ActionResult NewUser(string firstname, string lastname, string email, bool isadmin, string password)
        {
            var response = _statusRepository.ValidateUser(new AuthRequest { UserName = email, Password = password });
            if (response.ErrorCode == 2)
            {
                response = _statusRepository.NewUser(new User { FirstName = firstname, LastName = lastname, Email = email.ToLower(), IsAdmin = isadmin }, password);
                return Ok(response);

            }
            if(response.ErrorCode == 1) 
            {
                response.IsSuccess = false;
                response.Body = new { };
                response.Message = "Username and password is required";
                return BadRequest(response);
            }
            response.IsSuccess = false;
            response.ErrorCode = 3;
            response.Body = new { };
            response.Message = "Username allready in use!";
            return BadRequest(response);
        }

        [HttpPost("update/user")]
        public ActionResult UpdateUser(int id, string firstname, string lastname, string email, bool isadmin, string? password) 
        {
            User user = new User { Id = id, FirstName = firstname, LastName = lastname, Email = email.ToLower(), IsAdmin = isadmin };
            var response = _statusRepository.UpdateUser(user, password);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("remove/user")]
        public ActionResult DeleteUser(int userId)
        {
            var result = _statusRepository.DeleteUser(userId);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("get/tokens")]
        public ActionResult GetTokens()
        {
            var result = _statusRepository.GetTokens();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("get/token")]
        public ActionResult GetToken(int id)
        {
            var result = _statusRepository.GetToken(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("remove/token")]
        public ActionResult DeleteToken(int id)
        {
            var result = _statusRepository.DeleteToken(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("clear/tokens")]
        public ActionResult DeleteAlTokens()
        {
            var result = _statusRepository.DeleteAllTokens();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
