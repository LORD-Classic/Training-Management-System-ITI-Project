using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository interface for Grade entity operations.
    /// Provides data access methods specific to grade management.
    /// </summary>
    public interface IGradeRepository : IRepository<Grade>
    {
        /// <summary>
        /// Gets all grades with session and trainee information
        /// </summary>
        /// <returns>Collection of grades with session and trainee details</returns>
        Task<IEnumerable<Grade>> GetGradesWithDetailsAsync();

        /// <summary>
        /// Gets grades for a specific session
        /// </summary>
        /// <param name="sessionId">The session ID to filter by</param>
        /// <returns>Collection of grades for the specified session</returns>
        Task<IEnumerable<Grade>> GetBySessionAsync(int sessionId);

        /// <summary>
        /// Gets grades for a specific trainee
        /// </summary>
        /// <param name="traineeId">The trainee ID to filter by</param>
        /// <returns>Collection of grades for the specified trainee</returns>
        Task<IEnumerable<Grade>> GetByTraineeAsync(int traineeId);

        /// <summary>
        /// Gets grades for a specific course (across all sessions)
        /// </summary>
        /// <param name="courseId">The course ID to filter by</param>
        /// <returns>Collection of grades for the specified course</returns>
        Task<IEnumerable<Grade>> GetByCourseAsync(int courseId);

        /// <summary>
        /// Gets the average grade for a specific trainee
        /// </summary>
        /// <param name="traineeId">The trainee ID to calculate average for</param>
        /// <returns>Average grade value for the trainee</returns>
        Task<double> GetAverageGradeForTraineeAsync(int traineeId);

        /// <summary>
        /// Gets the average grade for a specific session
        /// </summary>
        /// <param name="sessionId">The session ID to calculate average for</param>
        /// <returns>Average grade value for the session</returns>
        Task<double> GetAverageGradeForSessionAsync(int sessionId);

        /// <summary>
        /// Gets the average grade for a specific course
        /// </summary>
        /// <param name="courseId">The course ID to calculate average for</param>
        /// <returns>Average grade value for the course</returns>
        Task<double> GetAverageGradeForCourseAsync(int courseId);

        /// <summary>
        /// Checks if a grade already exists for a trainee in a specific session
        /// </summary>
        /// <param name="sessionId">The session ID to check</param>
        /// <param name="traineeId">The trainee ID to check</param>
        /// <param name="excludeGradeId">Grade ID to exclude from the check (for updates)</param>
        /// <returns>True if grade already exists, false otherwise</returns>
        Task<bool> GradeExistsForTraineeInSessionAsync(int sessionId, int traineeId, int? excludeGradeId = null);

        /// <summary>
        /// Gets grades within a specific grade range
        /// </summary>
        /// <param name="minGrade">Minimum grade value</param>
        /// <param name="maxGrade">Maximum grade value</param>
        /// <returns>Collection of grades within the specified range</returns>
        Task<IEnumerable<Grade>> GetByGradeRangeAsync(int minGrade, int maxGrade);
    }
}
