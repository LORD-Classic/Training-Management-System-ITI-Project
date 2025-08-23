using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training_Management_System_ITI_Project.Models
{
    /// <summary>
    /// Represents a grade for a trainee in a specific training session.
    /// Each grade is unique per trainee per session and has a value between 0-100.
    /// Grades are used to track trainee performance and progress.
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
        [Display(Name = "Session")]
        public int SessionId { get; set; }

        /// <summary>
        /// Foreign key to the User table - identifies which trainee received this grade
        /// </summary>
        [Required(ErrorMessage = "Trainee is required")]
        [Display(Name = "Trainee")]
        public int TraineeId { get; set; }

        /// <summary>
        /// Grade value - must be between 0 and 100
        /// </summary>
        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade Value")]
        public int Value { get; set; }

        /// <summary>
        /// Optional comments or feedback about the trainee's performance
        /// </summary>
        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters")]
        [Display(Name = "Comments")]
        [DataType(DataType.MultilineText)]
        public string? Comments { get; set; }

        /// <summary>
        /// Date when the grade was recorded
        /// </summary>
        [Display(Name = "Grade Date")]
        [DataType(DataType.DateTime)]
        public DateTime GradeDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date when the grade was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Navigation property to the associated Session entity
        /// </summary>
        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; } = null!;

        /// <summary>
        /// Navigation property to the associated Trainee User entity
        /// </summary>
        [ForeignKey("TraineeId")]
        public virtual User Trainee { get; set; } = null!;

        // Computed properties for grade analysis

        /// <summary>
        /// Computed property to determine grade letter (A, B, C, D, F)
        /// </summary>
        [NotMapped]
        public string GradeLetter
        {
            get
            {
                return Value switch
                {
                    >= 90 => "A",
                    >= 80 => "B",
                    >= 70 => "C",
                    >= 60 => "D",
                    _ => "F"
                };
            }
        }

        /// <summary>
        /// Computed property to determine if grade is passing (60 or above)
        /// </summary>
        [NotMapped]
        public bool IsPassing => Value >= 60;

        /// <summary>
        /// Computed property to determine grade quality (Excellent, Good, Satisfactory, Needs Improvement, Failing)
        /// </summary>
        [NotMapped]
        public string GradeQuality
        {
            get
            {
                return Value switch
                {
                    >= 90 => "Excellent",
                    >= 80 => "Good",
                    >= 70 => "Satisfactory",
                    >= 60 => "Needs Improvement",
                    _ => "Failing"
                };
            }
        }
    }
}
