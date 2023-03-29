using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Services;

namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class CodesController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public CodesController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet("get/codes")]
        public ActionResult Codes()
        {
            var codes = _statusRepository.GetCodes();
            return Ok(codes);
        }


    }
}
