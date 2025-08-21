using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Session>> GetSessionsWithCourseAsync()
        {
            return await _dbSet
                .Include(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<Session?> GetSessionWithCourseAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Session>> SearchByCourseNameAsync(string courseName)
        {
            if (string.IsNullOrEmpty(courseName))
                return await GetSessionsWithCourseAsync();

            return await _dbSet
                .Include(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .Where(s => s.Course.Name.Contains(courseName))
                .ToListAsync();
        }

        public override async Task<Session?> GetByIdAsync(int id)
        {
            return await GetSessionWithCourseAsync(id);
        }
    }
}
