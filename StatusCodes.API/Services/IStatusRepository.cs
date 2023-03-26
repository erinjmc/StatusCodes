using Microsoft.AspNetCore.Mvc;
using StatusCodes.API.Models;
using System.Security.Claims;

namespace StatusCodes.API.Services
{
    public interface IStatusRepository
    {
        IEnumerable<StatusCode> GetCodes();
        IEnumerable<User> GetUsers();
        User GetUser(string username);
        User NewUser(User user, string password);
        string ValidateUser(AuthRequest creds);
        bool InvalidateUser(List<Claim> claims);
    }
}
