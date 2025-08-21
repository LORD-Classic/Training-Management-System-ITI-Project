using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetCoursesWithInstructorAsync()
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _dbSet.Where(c => c.Name.ToLower() == name.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Course>> SearchByNameOrCategoryAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetCoursesWithInstructorAsync();

            return await _dbSet
                .Include(c => c.Instructor)
                .Where(c => c.Name.Contains(searchTerm) || c.Category.Contains(searchTerm))
                .ToListAsync();
        }

        public override async Task<Course?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
