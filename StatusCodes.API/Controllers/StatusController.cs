using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Services;

namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet("codes")]
        public ActionResult Codes()
        {
            var codes = _statusRepository.GetCodes();
            return Ok(codes);
        }


    }
}
