using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository implementation for Course entity operations.
    /// Provides data access methods for course management functionality.
    /// </summary>
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all courses with instructor information
        /// </summary>
        public async Task<IEnumerable<Course>> GetCoursesWithInstructorAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets courses by category
        /// </summary>
        public async Task<IEnumerable<Course>> GetByCategoryAsync(string category)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.Category.ToLower() == category.ToLower())
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets courses assigned to a specific instructor
        /// </summary>
        public async Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.InstructorId == instructorId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Searches courses by name or category
        /// </summary>
        public async Task<IEnumerable<Course>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetCoursesWithInstructorAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.Name.ToLower().Contains(searchTerm) || 
                           c.Category.ToLower().Contains(searchTerm))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a course name is already in use
        /// </summary>
        public async Task<bool> IsNameInUseAsync(string name, int? excludeCourseId = null)
        {
            if (excludeCourseId.HasValue)
            {
                return await _context.Courses
                    .AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != excludeCourseId.Value);
            }

            return await _context.Courses
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets all available categories in the system
        /// </summary>
        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Courses
                .Select(c => c.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
}
