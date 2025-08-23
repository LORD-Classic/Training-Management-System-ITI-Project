using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Represents a training session for a specific course.
  /// Each session has a start and end date, and belongs to exactly one course.
  /// </summary>
  public class Session
  {
    /// <summary>
    /// Primary key - unique identifier for the session
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the Course table - identifies which course this session belongs to
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// When the training session starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the training session ends
    /// </summary>
    public DateTime EndDate { get; set; }
  }
}
