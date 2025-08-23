using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    /// <summary>
    /// View model for user creation and editing operations.
    /// Contains validation attributes and user input properties.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// User ID for editing operations
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's full name - required field with length validation
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// User's email address - must be unique and in valid format
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's role in the system - determines access permissions
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "User Role")]
        public UserRole Role { get; set; }

        /// <summary>
        /// Password for new users - required for creation
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Password confirmation for new users
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this is a new user (for conditional validation)
        /// </summary>
        public bool IsNewUser => Id == 0;
    }

    /// <summary>
    /// View model for user listing and search operations.
    /// Contains properties for displaying user information in lists.
    /// </summary>
    public class UserListViewModel
    {
        /// <summary>
        /// Collection of users to display
        /// </summary>
        public IEnumerable<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// Search term for filtering users
        /// </summary>
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Selected role for filtering users
        /// </summary>
        public UserRole? SelectedRole { get; set; }

        /// <summary>
        /// Total count of users matching the current filters
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number for pagination
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Number of users per page
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
