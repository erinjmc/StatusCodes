using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;
using StatusCodes.API.Services;

namespace StatusCodes.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class TokenController : Controller
    {
        private readonly IStatusRepository _statusRepository;

        public TokenController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet("tokens")]
        public async Task<ActionResult> GetTokens()
        {
            var result = await _statusRepository.GetTokens();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("token")]
        public async Task<ActionResult> GetToken(TokenDto token)
        {
            var result = await _statusRepository.GetToken(token);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("token")]
        public ActionResult DeleteToken(TokenDto token)
        {
            var result = _statusRepository.DeleteToken(token);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("tokens")]
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
