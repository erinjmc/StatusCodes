using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Entities;
using StatusCodes.API.Models;
using System.Security.Claims;

namespace StatusCodes.API.Services
{
    public interface IStatusRepository
    {
        Task<ResultDto> GetCodes();
        Task<ResultDto> GetTokens();
        Task<ResultDto> GetToken(TokenDto token);
        ResultDto DeleteToken(TokenDto token);
        ResultDto DeleteAllTokens();
        Task<ResultDto> GetUsers();
        Task<ResultDto> GetUser(UserDto user);
        ResultDto NewUser(UserDto user);
        ResultDto UpdateUser(UserDto user);
        ResultDto ValidateUser(AuthReqDto creds);
        ResultDto AuthLogonUser(AuthReqDto creds);
        ResultDto InvalidateUser(UserDto user);
        ResultDto DeleteUser(UserDto user);
    }
}
