using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository implementation for Session entity operations.
    /// Provides data access methods for session management functionality.
    /// </summary>
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all sessions with course information
        /// </summary>
        public async Task<IEnumerable<Session>> GetSessionsWithCourseAsync()
        {
            return await _context.Sessions
                .Include(s => s.Course)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets sessions for a specific course
        /// </summary>
        public async Task<IEnumerable<Session>> GetByCourseAsync(int courseId)
        {
            return await _context.Sessions
                .Include(s => s.Course)
                .Where(s => s.CourseId == courseId)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets sessions within a date range
        /// </summary>
        public async Task<IEnumerable<Session>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sessions
                .Include(s => s.Course)
                .Where(s => s.StartDate >= startDate && s.StartDate <= endDate)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets upcoming sessions (sessions starting from today onwards)
        /// </summary>
        public async Task<IEnumerable<Session>> GetUpcomingSessionsAsync()
        {
            var today = DateTime.Today;
            return await _context.Sessions
                .Include(s => s.Course)
                .Where(s => s.StartDate >= today)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Searches sessions by course name
        /// </summary>
        public async Task<IEnumerable<Session>> SearchByCourseNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetSessionsWithCourseAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Sessions
                .Include(s => s.Course)
                .Where(s => s.Course.Name.ToLower().Contains(searchTerm))
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets sessions for today
        /// </summary>
        public async Task<IEnumerable<Session>> GetTodaySessionsAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            
            return await _context.Sessions
                .Include(s => s.Course)
                .Where(s => s.StartDate >= today && s.StartDate < tomorrow)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets sessions for a specific date
        /// </summary>
        public async Task<IEnumerable<Session>> GetByDateAsync(DateTime date)
        {
            var nextDay = date.AddDays(1);
            
            return await _context.Sessions
                .Include(s => s.Course)
                .Where(s => s.StartDate >= date && s.StartDate < nextDay)
                .OrderBy(s => s.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Override to include course information when getting by ID
        /// </summary>
        public override async Task<Session?> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
