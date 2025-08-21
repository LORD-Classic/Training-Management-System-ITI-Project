using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Represents a training course entity in the system.
  /// Each course has a unique name, belongs to a category, and can be assigned to an instructor.
  /// </summary>
  public class Course
  {
    /// <summary>
    /// Primary key - unique identifier for the course
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Course name - must be unique across all courses
    /// Validated to be required and between 3-50 characters
    /// </summary>
    [Required(ErrorMessage = "Course name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Course category (e.g., "Programming", "Design", "Management")
    /// Required field for organizing and searching courses
    /// </summary>
    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the User table - references the instructor assigned to this course
    /// Nullable because a course might not have an instructor assigned initially
    /// </summary>
    public int? InstructorId { get; set; }

    // Navigation properties for Entity Framework relationships

    /// <summary>
    /// Navigation property to the instructor User entity
    /// Enables lazy loading of instructor details when accessing course data
    /// </summary>
    public virtual User? Instructor { get; set; }

    /// <summary>
    /// Collection of training sessions associated with this course
    /// One course can have multiple sessions scheduled at different times
    /// </summary>
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
  }
}
