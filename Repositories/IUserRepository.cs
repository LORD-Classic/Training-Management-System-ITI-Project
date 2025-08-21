using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
    }
}
