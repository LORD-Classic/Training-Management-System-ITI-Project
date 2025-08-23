using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    /// <summary>
    /// View model for course creation and editing operations.
    /// Contains validation attributes and course input properties.
    /// </summary>
    public class CourseViewModel
    {
        /// <summary>
        /// Course ID for editing operations
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Course name - must be unique across all courses
        /// </summary>
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 50 characters")]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Course category - required field for organizing courses
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Instructor ID - optional field for assigning an instructor to the course
        /// </summary>
        [Display(Name = "Instructor")]
        public int? InstructorId { get; set; }

        /// <summary>
        /// Instructor name for display purposes
        /// </summary>
        [Display(Name = "Instructor Name")]
        public string? InstructorName { get; set; }

        /// <summary>
        /// Indicates if this is a new course (for conditional validation)
        /// </summary>
        public bool IsNewCourse => Id == 0;
    }

    /// <summary>
    /// View model for course listing and search operations.
    /// Contains properties for displaying course information in lists.
    /// </summary>
    public class CourseListViewModel
    {
        /// <summary>
        /// Collection of courses to display
        /// </summary>
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();

        /// <summary>
        /// Search term for filtering courses
        /// </summary>
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Selected category for filtering courses
        /// </summary>
        public string? SelectedCategory { get; set; }

        /// <summary>
        /// Total count of courses matching the current filters
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Available categories for filtering
        /// </summary>
        public IEnumerable<string> AvailableCategories { get; set; } = new List<string>();
    }
}
