using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    public interface IGradeRepository : IRepository<Grade>
    {
        Task<IEnumerable<Grade>> GetGradesByTraineeAsync(int traineeId);
        Task<IEnumerable<Grade>> GetGradesBySessionAsync(int sessionId);
        Task<Grade?> GetGradeBySessionAndTraineeAsync(int sessionId, int traineeId);
        Task<IEnumerable<Grade>> GetGradesWithDetailsAsync();
    }
}
