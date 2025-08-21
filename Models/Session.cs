using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Models
{
  /// <summary>
  /// Represents a training session for a specific course.
  /// Each session has a start and end date, and belongs to exactly one course.
  /// Trainees attend sessions and receive grades for their performance.
  /// </summary>
  public class Session
  {
    /// <summary>
    /// Primary key - unique identifier for the session
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the Course table - identifies which course this session belongs to
    /// Required field as every session must be associated with a course
    /// </summary>
    [Required(ErrorMessage = "Course is required")]
    public int CourseId { get; set; }

    /// <summary>
    /// When the training session starts
    /// Must be a future date (cannot schedule sessions in the past)
    /// Custom validation attribute ensures business rule compliance
    /// </summary>
    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.DateTime)]
    [FutureDate(ErrorMessage = "Start date cannot be in the past")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the training session ends
    /// Must be after the start date to ensure logical session duration
    /// Custom validation attribute compares with StartDate property
    /// </summary>
    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.DateTime)]
    [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
    public DateTime EndDate { get; set; }

    // Navigation properties for Entity Framework relationships

    /// <summary>
    /// Navigation property to the associated Course entity
    /// Enables access to course details when working with session data
    /// </summary>
    public virtual Course Course { get; set; } = null!;

    /// <summary>
    /// Collection of grades recorded for trainees in this session
    /// One session can have multiple grades (one per trainee participant)
    /// </summary>
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
  }
}
