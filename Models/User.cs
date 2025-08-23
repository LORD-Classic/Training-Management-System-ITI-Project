using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training_Management_System_ITI_Project.Models
{
    /// <summary>
    /// Represents a user in the training management system.
    /// Users can have different roles: Admin, Instructor, or Trainee.
    /// Each user has a unique email address and can participate in various system activities.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary key - unique identifier for the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's full name - required and must be between 3-50 characters
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// User's email address - required, must be unique, and must be a valid email format
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's role in the system - determines access levels and capabilities
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "User Role")]
        public UserRole Role { get; set; }

        /// <summary>
        /// Indicates whether the user account is active
        /// </summary>
        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When the user account was created
        /// </summary>
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// When the user account was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Collection of courses where this user is the instructor
        /// </summary>
        public virtual ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();

        /// <summary>
        /// Collection of grades received by this user (if they are a trainee)
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }

    /// <summary>
    /// Enumeration of possible user roles in the system.
    /// Each role has different permissions and access levels.
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Administrator role - full system access and control
        /// </summary>
        [Display(Name = "Administrator")]
        Admin = 1,

        /// <summary>
        /// Instructor role - can manage courses and sessions, view grades
        /// </summary>
        [Display(Name = "Instructor")]
        Instructor = 2,

        /// <summary>
        /// Trainee role - can view courses, sessions, and their own grades
        /// </summary>
        [Display(Name = "Trainee")]
        Trainee = 3
    }
}
