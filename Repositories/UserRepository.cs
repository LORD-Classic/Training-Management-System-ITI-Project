using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository implementation for User entity operations.
    /// Provides data access methods for user management functionality.
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets all users with a specific role
        /// </summary>
        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all instructors (users with Instructor role)
        /// </summary>
        public async Task<IEnumerable<User>> GetInstructorsAsync()
        {
            return await GetByRoleAsync(UserRole.Instructor);
        }

        /// <summary>
        /// Gets all trainees (users with Trainee role)
        /// </summary>
        public async Task<IEnumerable<User>> GetTraineesAsync()
        {
            return await GetByRoleAsync(UserRole.Trainee);
        }

        /// <summary>
        /// Gets all administrators (users with Admin role)
        /// </summary>
        public async Task<IEnumerable<User>> GetAdminsAsync()
        {
            return await GetByRoleAsync(UserRole.Admin);
        }

        /// <summary>
        /// Searches users by name or email
        /// </summary>
        public async Task<IEnumerable<User>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Users
                .Where(u => u.Name.ToLower().Contains(searchTerm) || 
                           u.Email.ToLower().Contains(searchTerm))
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if an email is already in use by another user
        /// </summary>
        public async Task<bool> IsEmailInUseAsync(string email, int? excludeUserId = null)
        {
            if (excludeUserId.HasValue)
            {
                return await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeUserId.Value);
            }

            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
