using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository implementation for Grade entity operations.
    /// Provides data access methods for grade management functionality.
    /// </summary>
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all grades with session and trainee information
        /// </summary>
        public async Task<IEnumerable<Grade>> GetGradesWithDetailsAsync()
        {
            return await _context.Grades
                .Include(g => g.Session)
                    .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .OrderByDescending(g => g.Value)
                .ToListAsync();
        }

        /// <summary>
        /// Gets grades for a specific session
        /// </summary>
        public async Task<IEnumerable<Grade>> GetBySessionAsync(int sessionId)
        {
            return await _context.Grades
                .Include(g => g.Trainee)
                .Where(g => g.SessionId == sessionId)
                .OrderByDescending(g => g.Value)
                .ToListAsync();
        }

        /// <summary>
        /// Gets grades for a specific trainee
        /// </summary>
        public async Task<IEnumerable<Grade>> GetByTraineeAsync(int traineeId)
        {
            return await _context.Grades
                .Include(g => g.Session)
                    .ThenInclude(s => s.Course)
                .Where(g => g.TraineeId == traineeId)
                .OrderByDescending(g => g.Session.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets grades for a specific course (across all sessions)
        /// </summary>
        public async Task<IEnumerable<Grade>> GetByCourseAsync(int courseId)
        {
            return await _context.Grades
                .Include(g => g.Session)
                .Include(g => g.Trainee)
                .Where(g => g.Session.CourseId == courseId)
                .OrderByDescending(g => g.Value)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the average grade for a specific trainee
        /// </summary>
        public async Task<double> GetAverageGradeForTraineeAsync(int traineeId)
        {
            var grades = await _context.Grades
                .Where(g => g.TraineeId == traineeId)
                .Select(g => g.Value)
                .ToListAsync();

            return grades.Any() ? grades.Average() : 0.0;
        }

        /// <summary>
        /// Gets the average grade for a specific session
        /// </summary>
        public async Task<double> GetAverageGradeForSessionAsync(int sessionId)
        {
            var grades = await _context.Grades
                .Where(g => g.SessionId == sessionId)
                .Select(g => g.Value)
                .ToListAsync();

            return grades.Any() ? grades.Average() : 0.0;
        }

        /// <summary>
        /// Gets the average grade for a specific course
        /// </summary>
        public async Task<double> GetAverageGradeForCourseAsync(int courseId)
        {
            var grades = await _context.Grades
                .Include(g => g.Session)
                .Where(g => g.Session.CourseId == courseId)
                .Select(g => g.Value)
                .ToListAsync();

            return grades.Any() ? grades.Average() : 0.0;
        }

        /// <summary>
        /// Checks if a grade already exists for a trainee in a specific session
        /// </summary>
        public async Task<bool> GradeExistsForTraineeInSessionAsync(int sessionId, int traineeId, int? excludeGradeId = null)
        {
            var query = _context.Grades
                .Where(g => g.SessionId == sessionId && g.TraineeId == traineeId);

            if (excludeGradeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeGradeId.Value);
            }

            return await query.AnyAsync();
        }

        /// <summary>
        /// Gets grades within a specific grade range
        /// </summary>
        public async Task<IEnumerable<Grade>> GetByGradeRangeAsync(int minGrade, int maxGrade)
        {
            return await _context.Grades
                .Include(g => g.Session)
                    .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .Where(g => g.Value >= minGrade && g.Value <= maxGrade)
                .OrderByDescending(g => g.Value)
                .ToListAsync();
        }

        /// <summary>
        /// Override to include session and trainee information when getting by ID
        /// </summary>
        public override async Task<Grade?> GetByIdAsync(int id)
        {
            return await _context.Grades
                .Include(g => g.Session)
                    .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
