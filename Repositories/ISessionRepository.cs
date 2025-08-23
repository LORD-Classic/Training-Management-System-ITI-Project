using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository interface for Session entity operations.
    /// Provides data access methods specific to session management.
    /// </summary>
    public interface ISessionRepository : IRepository<Session>
    {
        /// <summary>
        /// Gets all sessions with course information
        /// </summary>
        /// <returns>Collection of sessions with course details</returns>
        Task<IEnumerable<Session>> GetSessionsWithCourseAsync();

        /// <summary>
        /// Gets sessions for a specific course
        /// </summary>
        /// <param name="courseId">The course ID to filter by</param>
        /// <returns>Collection of sessions for the specified course</returns>
        Task<IEnumerable<Session>> GetByCourseAsync(int courseId);

        /// <summary>
        /// Gets sessions within a date range
        /// </summary>
        /// <param name="startDate">Start date for the range</param>
        /// <param name="endDate">End date for the range</param>
        /// <returns>Collection of sessions within the date range</returns>
        Task<IEnumerable<Session>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets upcoming sessions (sessions starting from today onwards)
        /// </summary>
        /// <returns>Collection of upcoming sessions</returns>
        Task<IEnumerable<Session>> GetUpcomingSessionsAsync();

        /// <summary>
        /// Searches sessions by course name
        /// </summary>
        /// <param name="searchTerm">The search term to look for in course names</param>
        /// <returns>Collection of sessions matching the search criteria</returns>
        Task<IEnumerable<Session>> SearchByCourseNameAsync(string searchTerm);

        /// <summary>
        /// Gets sessions for today
        /// </summary>
        /// <returns>Collection of sessions scheduled for today</returns>
        Task<IEnumerable<Session>> GetTodaySessionsAsync();

        /// <summary>
        /// Gets sessions for a specific date
        /// </summary>
        /// <param name="date">The date to get sessions for</param>
        /// <returns>Collection of sessions for the specified date</returns>
        Task<IEnumerable<Session>> GetByDateAsync(DateTime date);
    }
}
