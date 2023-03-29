using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;
using System.Security.Claims;

namespace StatusCodes.API.Services
{
    public interface IStatusRepository
    {
        Result GetCodes();
        Result GetTokens();
        Result GetToken(int id);
        Result DeleteToken(int id);
        Result DeleteAllTokens();
        Result GetUsers();
        Result GetUser(int id);
        Result NewUser(User user, string password);
        Result UpdateUser(User user, string? password);
        Result ValidateUser(AuthRequest creds);
        Result AuthLogonUser(AuthRequest creds);
        Result InvalidateUser(int id);
        Result DeleteUser(int id);
    }
}
