using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
  /// <summary>
  /// View model for user login functionality
  /// Contains the necessary fields for authentication
  /// </summary>
  public class LoginViewModel
  {
    /// <summary>
    /// User's email address for login
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password for authentication
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether to remember the user's login session
    /// </summary>
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; } = false;

    /// <summary>
    /// Return URL after successful login
    /// </summary>
    public string? ReturnUrl { get; set; }
  }

  /// <summary>
  /// View model for user registration functionality
  /// Contains fields for creating new user accounts
  /// </summary>
  public class RegisterViewModel
  {
    /// <summary>
    /// User's full name
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Password confirmation to ensure accuracy
    /// </summary>
    [Required(ErrorMessage = "Password confirmation is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system
    /// Only SuperAdmin can create Admin accounts
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    [Display(Name = "Role")]
    public UserRole Role { get; set; } = UserRole.Trainee;
  }

  /// <summary>
  /// View model for changing password functionality
  /// </summary>
  public class ChangePasswordViewModel
  {
    /// <summary>
    /// Current password for verification
    /// </summary>
    [Required(ErrorMessage = "Current password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password confirmation
    /// </summary>
    [Required(ErrorMessage = "Password confirmation is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
  }

  /// <summary>
  /// View model for user profile management
  /// </summary>
  public class UserProfileViewModel
  {
    /// <summary>
    /// User ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// User's full name
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 50 characters")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's role
    /// </summary>
    [Display(Name = "Role")]
    public UserRole Role { get; set; }

    /// <summary>
    /// Whether the account is active
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    /// <summary>
    /// When the account was created
    /// </summary>
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the account was last updated
    /// </summary>
    [Display(Name = "Last Updated")]
    public DateTime? UpdatedAt { get; set; }
  }

  /// <summary>
  /// View model for user management (admin functions)
  /// </summary>
  public class ManageUsersViewModel
  {
    /// <summary>
    /// List of users to display
    /// </summary>
    public List<UserProfileViewModel> Users { get; set; } = new List<UserProfileViewModel>();

    /// <summary>
    /// Search term for filtering users
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Role filter for users
    /// </summary>
    public UserRole? RoleFilter { get; set; }

    /// <summary>
    /// Status filter (active/inactive)
    /// </summary>
    public bool? IsActiveFilter { get; set; }
  }
}
