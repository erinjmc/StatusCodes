using Newtonsoft.Json;
using StatusCodes.API.Entities;
using StatusCodes.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace StatusCodes.API.DbContext
{
    public class DbInit
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            try
            {
                using (StreamReader reader = new StreamReader("codes.json"))
                {
                    StatusCodesDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<StatusCodesDbContext>();

                    if (!context.StatusCodes.Any())
                    {
                        string json = reader.ReadToEnd();
                        if (json != null)
                        {
                            List<InitCode> items = new List<InitCode>();
                            items = JsonConvert.DeserializeObject<List<InitCode>>(json);

                            if (items != null)
                            {
                                foreach (InitCode c in items)
                                {
                                    context.StatusCodes.Add(new Status { Code = c.Code, Description = c.Desc, Platform = c.Platform, PlatformCode = c.PlatformCode });
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                    if (!context.Users.Any())
                    {
                        var newuser = new User { FirstName = "Admin", LastName = "Admin", Email = "admin@admin.com", IsAdmin = true, Salt = Guid.NewGuid().ToString() };
                        var hashedPasasword = ComputeSha256Hash("password"+ newuser.Salt);
                        newuser.HashedPassword = hashedPasasword;
                        context.Users.Add(newuser);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //ToDo log error, raise alarm
            }

        }
        public static string ComputeSha256Hash(string hash)
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

        public class InitCode
        {
            public int PlatformCode { get; set; }
            public int Code { get; set; }
            public string Desc { get; set; } = string.Empty;
            public string? Platform { get; set; }
        }
    }
}
