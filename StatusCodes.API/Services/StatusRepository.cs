using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusCodes.API.DbContext;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using StatusCodes.API.Models;
using Microsoft.VisualBasic;
using Azure;
using StatusCodes.API.Models;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace StatusCodes.API.Services
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


        public Result GetCodes()
        {
            var response = new Result { Message = "GetCodes" };
            var codes = _context.StatusCodes.ToList();

            if(codes != null)
            {
                response.Body = codes;
                response.IsSuccess = true;

            }
            
            response.IsSuccess = false;
            return response;
        }

        public Result GetTokens()
        {
            var response = new Result { Message = "GetTokens" };
            var tokens = _context.Tokens.ToList();

            if (tokens != null)
            {
                response.Body = tokens;
                response.IsSuccess = true;
                return response;
            }

            response.IsSuccess = false;
            return response;
        }

        public Result GetToken(int id)
        {
            var response = new Result { Message = $"GetToken token id = {id}" };
            var token = _context.Tokens.SingleOrDefault(t => t.Id == id);

            if(token != null)
            {
                response.Body = token;
                response.IsSuccess = true;
                return response;
            }

            response.IsSuccess = false;
            return response;
        }

        public Result DeleteToken(int id) 
        {
            var response = new Result { Message = $"DeleteToken token id = {id}" };
            var token = _context.Tokens.SingleOrDefault(t =>t.Id == id);
            if (token != null) {
                try
                {
                    _context.Tokens.Remove(token);
                    _context.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.Message += ex.Message; 
                }
            }
            response.IsSuccess = false;
            return response;
        }
        
        public Result DeleteAllTokens()
        {
            var response = new Result { Message = "DeleteAllTokens token" };
            try
            {
                var tokens = _context.Tokens.ToList();
                _context.Tokens.RemoveRange(tokens);
                _context.SaveChanges();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message += ex.Message;
            }
 
            response.IsSuccess = false;
            return response;
        }

        public Result GetUsers()
        {
            var response = new Result { Message = "GetUsers" };
            var users = _context.Users.ToList();
            if(users != null)
            {       
                response.Body = users;
                response.IsSuccess = true;
                return response;
            }
            response.IsSuccess = false;
            return response;
        }

        public Result GetUser(int id)
        {
            var response = new Result { Message = "GetUserById" };
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                response.Body = user;
                response.IsSuccess = true;
                return response;
            }
            response.IsSuccess = false;
            return response;
        }

        public Result NewUser(User user, string password)
        {
            var response = new Result { Message = "NewUser" };
            user.Salt = Guid.NewGuid().ToString();
            user.HashedPassword = ComputeSha256Hash(password + user.Salt);

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                if (user != null)
                {
                    response.Body = user;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch(Exception ex)
            {
                response.Body = ex.Message;
            }
            response.IsSuccess = false;
            return response;
        }

        public Result UpdateUser(User updatedUserRecord, string? password)
        {
            var response = new Result { Message = "UpdateUser" };
            if (password != null)
            {
                updatedUserRecord.HashedPassword = ComputeSha256Hash(password);
            }
            try
            {
                var currentUserRocord = _context.Users.FirstOrDefault(u => u.Id == updatedUserRecord.Id);
                if (currentUserRocord != null)
                {
                    currentUserRocord.FirstName = updatedUserRecord.FirstName;
                    currentUserRocord.LastName = updatedUserRecord.LastName;
                    currentUserRocord.Email = updatedUserRecord.Email;
                    currentUserRocord.IsAdmin = updatedUserRecord.IsAdmin;
                    if(password != null) 
                    {
                        currentUserRocord.HashedPassword = updatedUserRecord.HashedPassword;
                    }
                    _context.SaveChanges();
                    response.Body = _context.Users.FirstOrDefault(u => u.Id == updatedUserRecord.Id); ;
                    response.IsSuccess = true;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message += ex.Message;
            }
            response.IsSuccess = false;
            return response;
        }

        public Result DeleteUser(int id)
        {
            var response = new Result { Message = "DeleteUser" };
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex) 
            {
                response.Body = ex.Message;
            }
            response.IsSuccess = _context.Users.Contains(user);
            return response;
        }

        public Result ValidateUser(AuthRequest creds) 
        {
            var response = new Result { Message = "ValidateUser" };
            if (string.IsNullOrEmpty(creds.Password) || string.IsNullOrEmpty(creds.UserName))
            {
                response.IsSuccess = false;
                response.ErrorCode = 1;
                response.Message += " - Missing username and / or password";
                return response;
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == creds.UserName);

            if (user == null)
            {
                response.ErrorCode = 2;
                response.Message += " - No such user";
                response.IsSuccess = false;
                return response;
            }

            response.ErrorCode = 0;
            response.Message += " - User found";
            response.IsSuccess = true;
            response.Body = user;
            return response;
        }

        public Result AuthLogonUser(AuthRequest creds)
        {
            var response = ValidateUser(creds);
            if(response.Body != null)
            {
                var user = (User)response.Body;
                if (user != null)
                {
                    try
                    {
                        if (user.HashedPassword == ComputeSha256Hash(creds.Password + user.Salt))
                        {
                            var token = BuildToken(user);
                            if (token != null)
                            {
                                _context.Tokens.Add(new Token { UserId = user.Id, Email = user.Email, TokenStr = token });
                                _context.SaveChanges();
                                response.Body = token;
                                response.IsSuccess = true;
                                return response;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Message += ex.Message;
                    }
                }
            }

            
            
            response.IsSuccess = false;
            return response;
        }

        public Result InvalidateUser(int id)
        {
            var response = new Result { Message = "InvalidateUser" };
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                try
                {
                    var cleaning = _context.Tokens.Where(t => t.UserId == user.Id).ToList();
                    _context.Tokens.RemoveRange(cleaning);
                    _context.SaveChanges();
                    response.IsSuccess = true;
                    return response;

                } catch (Exception ex)
                {
                    response.Message += ex.Message;
                }
                
                response.IsSuccess = user.Tokens.Count == 0;
            }
            return response;
        }

        public string ComputeSha256Hash(string hash)
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
