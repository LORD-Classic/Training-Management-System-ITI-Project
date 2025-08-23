using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Course is required")]
        public int CourseId { get; set; }

        /// <summary>
        /// When the training session starts
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// When the training session ends
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Navigation property to the associated Course entity
        /// </summary>
        public virtual Course Course { get; set; } = null!;
    }
}
