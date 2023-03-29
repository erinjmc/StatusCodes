﻿using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;
using StatusCodes.API.Services;
using System.Security.Claims;

namespace StatusCodes.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;

        public AuthController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }
      
        [HttpPost("logon")]
        public ActionResult Logon(string username, string password)
        {
            var result = _statusRepository.AuthLogonUser(new AuthRequest { UserName = username.ToLower(), Password = password });
            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpPost("logout")] 
        public ActionResult Logout(int id)
        {
            var result = _statusRepository.InvalidateUser(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NoContent();
        }

    }
}
