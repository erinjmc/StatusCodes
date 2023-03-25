using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;

namespace StatusCodes.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IStatusRepository statusRepository;

        public UserController(IStatusRepository _statusRepository)
        {
            statusRepository = _statusRepository;
        }


        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var codes = await statusRepository.GetUsers();
            return Ok(codes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var codes = await statusRepository.GetUser(id);
            return Ok(codes);
        }
    }
}
