using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;

namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Route("api/codes")]
    public class CodesController : ControllerBase
    {
        private readonly IStatusRepository statusRepository;

        public CodesController(IStatusRepository _statusRepository)
        {
            statusRepository = _statusRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Codes()
        {
            var codes = await statusRepository.GetCodes();
            return Ok(codes);
        }


    }
}
