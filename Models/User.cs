using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Enumeration defining the different user roles in the training management system
  /// </summary>
  public enum UserRole
  {
    /// <summary>
    /// Student/trainee who attends sessions and receives grades
    /// </summary>
    Trainee = 0,

    /// <summary>
    /// Course instructor who can manage assigned courses and sessions
    /// </summary>
    Instructor = 1,

    /// <summary>
    /// System administrator with access to most features
    /// </summary>
    Admin = 2
  }

  /// <summary>
  /// Represents a user in the training management system.
  /// Users can be trainees, instructors, or administrators.
  /// </summary>
  public class User
  {
    /// <summary>
    /// Primary key - unique identifier for the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's full name - required field with length validation
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User's email address - must be unique and in valid email format
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system - determines access permissions
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; }
  }
}
