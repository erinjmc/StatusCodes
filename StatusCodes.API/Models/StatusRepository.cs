using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusCodes.API.DbContext;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace StatusCodes.API.Models
{
    public class StatusRepository : IStatusRepository
    {
        private readonly StatusCodesDbContext _context;
        private readonly IConfiguration _configuration;

        public StatusRepository(StatusCodesDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IEnumerable<StatusCode> GetCodes()
        {
            return _context.StatusCodes.ToList();
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public User NewUser(User user, string password)
        {
            user.HashedPassword = ComputeSha256Hash(password + user.Email);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public string ValidateUser(AuthRequest creds)
        {
            string token = String.Empty;
            if (!String.IsNullOrEmpty(creds.Password) && !String.IsNullOrEmpty(creds.UserName))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == creds.UserName);

                if (user != null)
                {
                    if (user.HashedPassword == ComputeSha256Hash(creds.Password + creds.UserName))
                    {
                        token = BuildToken(user);
                    }
                }
            }           
            return token;
        }

        private string ComputeSha256Hash(string hash)
        {
            string hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hash));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                hashedPassword = builder.ToString();
            }
            return hashedPassword;
        }

        private string BuildToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
            claimsForToken.Add(new Claim("firstname", user.FirstName));
            claimsForToken.Add(new Claim("lastname", user.LastName));
            claimsForToken.Add(new Claim("username", user.Email));
            claimsForToken.Add(new Claim("isadmin", user.IsAdmin.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
