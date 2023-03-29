using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Entities;
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
        public async Task<ActionResult> GetUsers()
        {
            var response = await _statusRepository.GetUsers();
            if (response.IsSuccess)
            {
                return Ok(response); 
            }
            return NotFound(response);
        }

        [HttpGet("user")]
        public async Task<ActionResult> GetUser(UserDto findUser)
        {
            var response = await _statusRepository.GetUser(findUser);
            if (response == null)
            {
                return Ok(response);
            }
            return NotFound(response);
            
        }

        [HttpPost("user")]
        public ActionResult NewUser(UserDto newRecord)
        {
            var response = _statusRepository.ValidateUser(new AuthReqDto { UserName = newRecord.Email, Password = newRecord.NewPassword });
            if (response.ErrorCode == 2)
            {
                response = _statusRepository.NewUser(newRecord);
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

        [HttpPatch("user")]
        public ActionResult UpdateUser(UserDto changedRecord) 
        {
            var response = _statusRepository.UpdateUser(changedRecord);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("user")]
        public ActionResult DeleteUser(UserDto findUser)
        {
            var result = _statusRepository.DeleteUser(findUser);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
