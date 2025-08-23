using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{
    /// <summary>
    /// Represents a grade for a trainee in a specific training session.
    /// Each grade is unique per trainee per session and has a value between 0-100.
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Primary key - unique identifier for the grade
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the Session table - identifies which session this grade belongs to
        /// </summary>
        [Required(ErrorMessage = "Session is required")]
        public int SessionId { get; set; }

        /// <summary>
        /// Foreign key to the User table - identifies which trainee received this grade
        /// </summary>
        [Required(ErrorMessage = "Trainee is required")]
        public int TraineeId { get; set; }

        /// <summary>
        /// Grade value - must be between 0 and 100
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade Value")]
        public int Value { get; set; }

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Navigation property to the associated Session entity
        /// </summary>
        public virtual Session Session { get; set; } = null!;

        /// <summary>
        /// Navigation property to the associated Trainee User entity
        /// </summary>
        public virtual User Trainee { get; set; } = null!;
    }
}
