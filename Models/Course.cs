using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training_Management_System_ITI_Project.Models
{
    /// <summary>
    /// Represents a training course entity in the system.
    /// Each course has a unique name, belongs to a category, and can be assigned to an instructor.
    /// Courses can have multiple sessions scheduled at different times.
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Primary key - unique identifier for the course
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Course name - must be unique across all courses and between 3-50 characters
        /// </summary>
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 50 characters")]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Course category - required field for organizing and searching courses
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Category must be between 2 and 30 characters")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Course description - optional field for detailed course information
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        /// <summary>
        /// Course duration in hours - optional field for planning purposes
        /// </summary>
        [Range(1, 200, ErrorMessage = "Duration must be between 1 and 200 hours")]
        [Display(Name = "Duration (Hours)")]
        public int? DurationHours { get; set; }

        /// <summary>
        /// Maximum number of trainees allowed in the course
        /// </summary>
        [Range(1, 100, ErrorMessage = "Maximum trainees must be between 1 and 100")]
        [Display(Name = "Maximum Trainees")]
        public int? MaxTrainees { get; set; }

        /// <summary>
        /// Foreign key to the User table - references the instructor assigned to this course
        /// Nullable because a course might not have an instructor assigned initially
        /// </summary>
        [Display(Name = "Instructor")]
        public int? InstructorId { get; set; }

        /// <summary>
        /// Indicates whether the course is currently active and available for enrollment
        /// </summary>
        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When the course was created
        /// </summary>
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// When the course was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Navigation property to the instructor User entity
        /// Enables lazy loading of instructor details when accessing course data
        /// </summary>
        [ForeignKey("InstructorId")]
        public virtual User? Instructor { get; set; }

        /// <summary>
        /// Collection of training sessions associated with this course
        /// One course can have multiple sessions scheduled at different times
        /// </summary>
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
