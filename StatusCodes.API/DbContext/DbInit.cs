using Newtonsoft.Json;
using StatusCodes.API.Models;

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
                            List<InitStatusCode> items = new List<InitStatusCode>();
                            items = JsonConvert.DeserializeObject<List<InitStatusCode>>(json);

                            if (items != null)
                            {
                                foreach (InitStatusCode c in items)
                                {
                                    context.StatusCodes.Add(new StatusCode { Code = c.Code, Description = c.Desc, Platform = c.Platform, PlatformCode = c.PlatformCode });
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                    //if (!context.users.any())
                    //{
                    //    context.users.add(new user { firstname = "amin", lastname = "admin", email = "admin@business.com"});
                    //    context.savechanges();
                    //}
                }
            }
            catch (Exception ex)
            {
                //ToDo log error, raise alarm
            }

        }
    }
}
