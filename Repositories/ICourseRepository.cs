using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository interface for Course entity operations.
    /// Provides data access methods specific to course management.
    /// </summary>
    public interface ICourseRepository : IRepository<Course>
    {
        /// <summary>
        /// Gets all courses with instructor information
        /// </summary>
        /// <returns>Collection of courses with instructor details</returns>
        Task<IEnumerable<Course>> GetCoursesWithInstructorAsync();

        /// <summary>
        /// Gets courses by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of courses in the specified category</returns>
        Task<IEnumerable<Course>> GetByCategoryAsync(string category);

        /// <summary>
        /// Gets courses assigned to a specific instructor
        /// </summary>
        /// <param name="instructorId">The instructor ID to filter by</param>
        /// <returns>Collection of courses assigned to the instructor</returns>
        Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId);

        /// <summary>
        /// Searches courses by name or category
        /// </summary>
        /// <param name="searchTerm">The search term to look for in name or category</param>
        /// <returns>Collection of courses matching the search criteria</returns>
        Task<IEnumerable<Course>> SearchAsync(string searchTerm);

        /// <summary>
        /// Checks if a course name is already in use
        /// </summary>
        /// <param name="name">The course name to check</param>
        /// <param name="excludeCourseId">Course ID to exclude from the check (for updates)</param>
        /// <returns>True if name is already in use, false otherwise</returns>
        Task<bool> IsNameInUseAsync(string name, int? excludeCourseId = null);

        /// <summary>
        /// Gets all available categories in the system
        /// </summary>
        /// <returns>Collection of unique category names</returns>
        Task<IEnumerable<string>> GetCategoriesAsync();
    }
}
