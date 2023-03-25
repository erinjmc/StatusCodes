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

        public async Task<IEnumerable<StatusCode>> GetCodesAsync()
        {
            return await _context.StatusCodes.ToListAsync();
        }
    }
}
