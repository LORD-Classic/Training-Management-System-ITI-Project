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
        /// </summary>
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Course category (e.g., "Programming", "Design", "Management")
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to the User table - references the instructor assigned to this course
        /// </summary>
        public int? InstructorId { get; set; }

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Navigation property to the instructor User entity
        /// </summary>
        public virtual User? Instructor { get; set; }
    }
}
