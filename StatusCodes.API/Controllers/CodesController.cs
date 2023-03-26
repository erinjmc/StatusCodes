using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;

namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/codes")]
    public class CodesController : ControllerBase
    {
        private readonly IStatusRepository statusRepository;

        public CodesController(IStatusRepository _statusRepository)
        {
            statusRepository = _statusRepository;
        }

        [HttpGet]
        public ActionResult Codes()
        {
            var codes = statusRepository.GetCodes();
            return Ok(codes);
        }


    }
}
