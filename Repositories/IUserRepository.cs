using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    /// <summary>
    /// Repository interface for User entity operations.
    /// Provides data access methods specific to user management.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets all users with a specific role
        /// </summary>
        /// <param name="role">The role to filter by</param>
        /// <returns>Collection of users with the specified role</returns>
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);

        /// <summary>
        /// Gets all instructors (users with Instructor role)
        /// </summary>
        /// <returns>Collection of instructor users</returns>
        Task<IEnumerable<User>> GetInstructorsAsync();

        /// <summary>
        /// Gets all trainees (users with Trainee role)
        /// </summary>
        /// <returns>Collection of trainee users</returns>
        Task<IEnumerable<User>> GetTraineesAsync();

        /// <summary>
        /// Gets all administrators (users with Admin role)
        /// </summary>
        /// <returns>Collection of admin users</returns>
        Task<IEnumerable<User>> GetAdminsAsync();

        /// <summary>
        /// Searches users by name or email
        /// </summary>
        /// <param name="searchTerm">The search term to look for in name or email</param>
        /// <returns>Collection of users matching the search criteria</returns>
        Task<IEnumerable<User>> SearchAsync(string searchTerm);

        /// <summary>
        /// Checks if an email is already in use by another user
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <param name="excludeUserId">User ID to exclude from the check (for updates)</param>
        /// <returns>True if email is already in use, false otherwise</returns>
        Task<bool> IsEmailInUseAsync(string email, int? excludeUserId = null);
    }
}
