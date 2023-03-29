
using Microsoft.EntityFrameworkCore;
using StatusCodes.API.Entities;
using StatusCodes.API.Models;

namespace StatusCodes.API.DbContext
{
    public class StatusCodesDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public StatusCodesDbContext(DbContextOptions<StatusCodesDbContext> options) : base(options)
        {
        }

        public DbSet<StatusCode> StatusCodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
