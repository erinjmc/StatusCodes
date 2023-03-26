using Microsoft.AspNetCore.Mvc;

namespace StatusCodes.API.Models
{
    public interface IStatusRepository
    {
        IEnumerable<StatusCode> GetCodes();
        IEnumerable<User> GetUsers();
        User GetUser(string username);
        User NewUser(User user, string password);
        string ValidateUser(AuthRequest creds);
        //    string ComputeSha256Hash(string hash);
        //    Token BuildToken(User user);
    }
}
