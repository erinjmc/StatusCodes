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
using Newtonsoft.Json.Linq;
using System.Reflection;
using StatusCodes.API.Entities;

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


        public async Task<ResultDto> GetCodes()
        {
            var response = new ResultDto { Message = "GetCodes" };
            var codes = await _context.StatusCodes.ToListAsync();

            if (codes != null)
            {
                response.Body = codes;
                response.IsSuccess = true;
                return response;
            }

            response.IsSuccess = false;
            return response;
        }

        public async Task<ResultDto> GetTokens()
        {
            var response = new ResultDto { Message = "GetTokens" };
            var tokens = await _context.Tokens.ToListAsync();

            if (tokens != null)
            {
                response.Body = tokens;
                response.IsSuccess = true;
                return response;
            }

            response.IsSuccess = false;
            return response;
        }

        public async Task<ResultDto> GetToken(TokenDto findToken)
        {
            var response = new ResultDto { Message = $"GetToken token id {findToken.Id}" };
            var token = await _context.Tokens.SingleOrDefaultAsync(t => t.Id == findToken.Id);

            if(token != null)
            {
                response.Body = token;
                response.IsSuccess = true;
                return response;
            }

            response.IsSuccess = false;
            return response;
        }

        public ResultDto DeleteToken(TokenDto findToken) 
        {
            var response = new ResultDto { Message = $"DeleteToken token id = {findToken.Id}" };
            var token = _context.Tokens.SingleOrDefault(t =>t.Id == findToken.Id);
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
        
        public ResultDto DeleteAllTokens()
        {
            var response = new ResultDto { Message = "DeleteAllTokens token" };
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

        public async Task<ResultDto> GetUsers()
        {
            var response = new ResultDto { Message = "GetUsers" };
            var users = await _context.Users.ToListAsync();
            if(users != null)
            {       
                response.Body = users;
                response.IsSuccess = true;
                return response;
            }
            response.IsSuccess = false;
            return response;
        }

        public async Task<ResultDto> GetUser(UserDto findUser)
        {
            var response = new ResultDto { Message = "GetUserById" };
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == findUser.Id);
            if (user != null)
            {
                response.Body = user;
                response.IsSuccess = true;
                return response;
            }
            response.IsSuccess = false;
            return response;
        }

        public ResultDto NewUser(UserDto newRecord)
        {
            var response = new ResultDto { Message = "NewUser" };
            var user = new User { FirstName = newRecord.FirstName, LastName = newRecord.LastName, Email = newRecord.Email.ToLower(), IsAdmin = newRecord.PromoteAdmin };
            user.Salt = Guid.NewGuid().ToString();
            user.HashedPassword = ComputeSha256Hash(newRecord.NewPassword + user.Salt);

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

        public ResultDto UpdateUser(UserDto changedRecord)
        {
            var response = new ResultDto { Message = "UpdateUser" };
            try
            {
                var currentRocord = _context.Users.FirstOrDefault(u => u.Id == changedRecord.Id);
                if (currentRocord != null)
                {
                    if(currentRocord.FirstName != changedRecord.FirstName && changedRecord.FirstName != string.Empty)
                    {
                        currentRocord.FirstName = changedRecord.FirstName;
                    }

                    if( currentRocord.LastName != changedRecord.LastName && changedRecord.LastName != string.Empty)
                    {
                        currentRocord.LastName = changedRecord.LastName;
                    }

                    if (currentRocord.Email != changedRecord.Email && changedRecord.Email != string.Empty)
                    {
                        currentRocord.Email = changedRecord.Email;
                    }

                    if (changedRecord.PromoteAdmin)
                    {
                        currentRocord.IsAdmin = !currentRocord.IsAdmin;
                    }

                    if(!changedRecord.NewPassword.IsNullOrEmpty()) 
                    {
                        currentRocord.HashedPassword = ComputeSha256Hash(changedRecord.NewPassword);
                    }
                    _context.SaveChanges();
                    var result = _context.Users.FirstOrDefault(u => u.Id == changedRecord.Id);
                    response.Body = result;
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

        public ResultDto DeleteUser(UserDto findUser)
        {
            var response = new ResultDto { Message = "DeleteUser" };
            var user = _context.Users.FirstOrDefault(u => u.Id == findUser.Id);
            if (user != null)
            {
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
            }
            response.IsSuccess = _context.Users.Contains(user);
            return response;
        }

        public ResultDto ValidateUser(AuthReqDto creds) 
        {
            var response = new ResultDto { Message = "ValidateUser" };
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

        public ResultDto AuthLogonUser(AuthReqDto creds)
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

        public ResultDto InvalidateUser(UserDto findUser)
        {
            var response = new ResultDto { Message = "InvalidateUser" };
            var user = _context.Users.FirstOrDefault(u => u.Id == findUser.Id);
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
