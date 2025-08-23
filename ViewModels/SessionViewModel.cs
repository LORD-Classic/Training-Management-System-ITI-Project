using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    /// <summary>
    /// View model for session creation and editing operations.
    /// Contains validation attributes and session input properties.
    /// </summary>
    public class SessionViewModel
    {
        /// <summary>
        /// Session ID for editing operations
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Course ID - required field for identifying which course this session belongs to
        /// </summary>
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        /// <summary>
        /// Course name for display purposes
        /// </summary>
        [Display(Name = "Course Name")]
        public string? CourseName { get; set; }

        /// <summary>
        /// When the training session starts
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// When the training session ends
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date & Time")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Indicates if this is a new session (for conditional validation)
        /// </summary>
        public bool IsNewSession => Id == 0;
    }

    /// <summary>
    /// View model for session listing and search operations.
    /// Contains properties for displaying session information in lists.
    /// </summary>
    public class SessionListViewModel
    {
        /// <summary>
        /// Collection of sessions to display
        /// </summary>
        public IEnumerable<Session> Sessions { get; set; } = new List<Session>();

        /// <summary>
        /// Search term for filtering sessions by course name
        /// </summary>
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Selected course for filtering sessions
        /// </summary>
        public int? SelectedCourseId { get; set; }

        /// <summary>
        /// Selected date for filtering sessions
        /// </summary>
        public DateTime? SelectedDate { get; set; }

        /// <summary>
        /// Total count of sessions matching the current filters
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Available courses for filtering
        /// </summary>
        public IEnumerable<Course> AvailableCourses { get; set; } = new List<Course>();
    }
}
