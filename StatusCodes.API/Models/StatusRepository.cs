using Microsoft.EntityFrameworkCore;
using StatusCodes.API.DbContext;

namespace StatusCodes.API.Models
{
    public class StatusRepository : IStatusRepository
    {
        private readonly StatusCodesDbContext _context;

        public StatusRepository(StatusCodesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StatusCode>> GetCodes()
        {
            return await _context.StatusCodes.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
