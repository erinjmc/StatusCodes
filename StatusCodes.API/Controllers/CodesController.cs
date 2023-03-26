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
        public ActionResult Codes()
        {
            var codes = statusRepository.GetCodes();
            return Ok(codes);
        }


    }
}
