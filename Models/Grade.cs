using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Represents a grade record for a trainee's performance in a specific session.
  /// Each grade links a trainee to a session with a numerical score (0-100).
  /// </summary>
  public class Grade
  {
    /// <summary>
    /// Primary key - unique identifier for the grade record
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the Session table - identifies which session this grade belongs to
    /// </summary>
    public int SessionId { get; set; }

    /// <summary>
    /// Foreign key to the User table - identifies which trainee received this grade
    /// </summary>
    public int TraineeId { get; set; }

    /// <summary>
    /// The numerical grade value (score) for the trainee's performance
    /// </summary>
    public int Value { get; set; }
  }
}
