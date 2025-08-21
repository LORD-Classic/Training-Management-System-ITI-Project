using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    public interface ISessionRepository : IRepository<Session>
    {
        Task<IEnumerable<Session>> SearchByCourseNameAsync(string courseName);
        Task<IEnumerable<Session>> GetSessionsWithCourseAsync();
        Task<Session?> GetSessionWithCourseAsync(int id);
    }
}
