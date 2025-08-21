using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Represents a grade record for a trainee's performance in a specific session.
  /// Each grade links a trainee to a session with a numerical score (0-100).
  /// This allows tracking of individual trainee progress across different sessions.
  /// </summary>
  public class Grade
  {
    /// <summary>
    /// Primary key - unique identifier for the grade record
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the Session table - identifies which session this grade belongs to
    /// Required field as every grade must be associated with a specific session
    /// </summary>
    [Required(ErrorMessage = "Session is required")]
    public int SessionId { get; set; }

    /// <summary>
    /// Foreign key to the User table - identifies which trainee received this grade
    /// Must reference a user with the 'Trainee' role
    /// Required field as every grade must be assigned to a specific trainee
    /// </summary>
    [Required(ErrorMessage = "Trainee is required")]
    public int TraineeId { get; set; }

    /// <summary>
    /// The numerical grade value (score) for the trainee's performance
    /// Must be between 0 and 100 inclusive, following standard grading scale
    /// Required field for meaningful grade tracking
    /// </summary>
    [Required(ErrorMessage = "Grade value is required")]
    [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
    public int Value { get; set; }

    // Navigation properties for Entity Framework relationships

    /// <summary>
    /// Navigation property to the associated Session entity
    /// Enables access to session details (course, dates) when working with grade data
    /// </summary>
    public virtual Session Session { get; set; } = null!;

    /// <summary>
    /// Navigation property to the Trainee (User) entity
    /// Enables access to trainee details when working with grade data
    /// </summary>
    public virtual User Trainee { get; set; } = null!;
  }
}
