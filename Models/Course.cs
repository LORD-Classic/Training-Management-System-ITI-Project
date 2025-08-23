using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Represents a training course entity in the system.
  /// Each course has a name, belongs to a category, and can be assigned to an instructor.
  /// </summary>
  public class Course
  {
    /// <summary>
    /// Primary key - unique identifier for the course
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Course name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Course category (e.g., "Programming", "Design", "Management")
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the User table - references the instructor assigned to this course
    /// </summary>
    public int? InstructorId { get; set; }
  }
}
