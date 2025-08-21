using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> SearchByNameOrCategoryAsync(string searchTerm);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<IEnumerable<Course>> GetCoursesWithInstructorAsync();
    }
}
