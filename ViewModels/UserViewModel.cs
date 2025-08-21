using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public UserRole Role { get; set; }
    }

    public class UserListViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public UserRole? FilterByRole { get; set; }
    }
}
