using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Enumeration defining the different user roles in the training management system
  /// Now includes hierarchical role structure with Super Admin at the top
  /// </summary>
  public enum UserRole
  {
    /// <summary>
    /// Student/trainee who attends sessions and receives grades
    /// Lowest privilege level
    /// </summary>
    Trainee = 0,

    /// <summary>
    /// Course instructor who can manage assigned courses and sessions
    /// Can manage their own courses and sessions
    /// </summary>
    Instructor = 1,

    /// <summary>
    /// System administrator with access to most features
    /// Can manage courses, sessions, and instructors
    /// </summary>
    Admin = 2,

    /// <summary>
    /// Super administrator with full system access
    /// Can manage everything including other admins
    /// </summary>
    SuperAdmin = 3
  }

  /// <summary>
  /// Represents an application user extending ASP.NET Core Identity.
  /// Integrates with the authentication system while maintaining training-specific properties.
  /// Users have hierarchical roles: SuperAdmin > Admin > Instructor > Trainee
  /// </summary>
  public class ApplicationUser : IdentityUser
  {
    /// <summary>
    /// User's full name - required field with length validation
    /// Must be between 3 and 50 characters for data consistency
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system with hierarchical permissions
    /// Determines what features and data the user can access
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; } = UserRole.Trainee;

    /// <summary>
    /// Indicates if the user account is active
    /// Allows for soft deletion and account suspension
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the user account was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the user account was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties for Entity Framework relationships

    /// <summary>
    /// Collection of courses where this user serves as an instructor
    /// Only relevant for users with Instructor role or higher
    /// </summary>
    public virtual ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();

    /// <summary>
    /// Collection of grades received by this user when they are a trainee
    /// Only relevant for users with Trainee role
    /// </summary>
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
  }

  /// <summary>
  /// Legacy User model for backward compatibility
  /// Maps to ApplicationUser for existing relationships
  /// </summary>
  public class User
  {
    /// <summary>
    /// Primary key - unique identifier for the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's full name - required field with length validation
    /// Must be between 3 and 50 characters for data consistency
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User's email address - must be unique and in valid email format
    /// Used for identification and communication purposes
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system (Admin, Instructor, or Trainee)
    /// Determines what features and data the user can access
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; }

    // Navigation properties for Entity Framework relationships

    /// <summary>
    /// Collection of courses where this user serves as an instructor
    /// Only relevant for users with Instructor role
    /// </summary>
    public virtual ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();

    /// <summary>
    /// Collection of grades received by this user when they are a trainee
    /// Only relevant for users with Trainee role
    /// </summary>
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
  }
}
