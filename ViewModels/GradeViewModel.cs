using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    /// <summary>
    /// View model for grade creation and editing operations.
    /// Contains validation attributes and grade input properties.
    /// </summary>
    public class GradeViewModel
    {
        /// <summary>
        /// Grade ID for editing operations
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Session ID - required field for identifying which session this grade belongs to
        /// </summary>
        [Required(ErrorMessage = "Session is required")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }

        /// <summary>
        /// Trainee ID - required field for identifying which trainee received this grade
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

        // Display properties for better user experience

        /// <summary>
        /// Session name for display purposes
        /// </summary>
        [Display(Name = "Session")]
        public string? SessionName { get; set; }

        /// <summary>
        /// Course name for display purposes
        /// </summary>
        [Display(Name = "Course")]
        public string? CourseName { get; set; }

        /// <summary>
        /// Trainee name for display purposes
        /// </summary>
        [Display(Name = "Trainee")]
        public string? TraineeName { get; set; }

        /// <summary>
        /// Indicates if this is a new grade (for conditional validation)
        /// </summary>
        public bool IsNewGrade => Id == 0;
    }

    /// <summary>
    /// View model for grade listing and search operations.
    /// Contains properties for displaying grade information in lists.
    /// </summary>
    public class GradeListViewModel
    {
        /// <summary>
        /// Collection of grades to display
        /// </summary>
        public IEnumerable<Grade> Grades { get; set; } = new List<Grade>();

        /// <summary>
        /// Selected session for filtering grades
        /// </summary>
        public int? SelectedSessionId { get; set; }

        /// <summary>
        /// Selected trainee for filtering grades
        /// </summary>
        public int? SelectedTraineeId { get; set; }

        /// <summary>
        /// Selected course for filtering grades
        /// </summary>
        public int? SelectedCourseId { get; set; }

        /// <summary>
        /// Minimum grade value for filtering
        /// </summary>
        public int? MinGrade { get; set; }

        /// <summary>
        /// Maximum grade value for filtering
        /// </summary>
        public int? MaxGrade { get; set; }

        /// <summary>
        /// Total count of grades matching the current filters
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Available sessions for filtering
        /// </summary>
        public IEnumerable<Session> AvailableSessions { get; set; } = new List<Session>();

        /// <summary>
        /// Available trainees for filtering
        /// </summary>
        public IEnumerable<User> AvailableTrainees { get; set; } = new List<User>();

        /// <summary>
        /// Available courses for filtering
        /// </summary>
        public IEnumerable<Course> AvailableCourses { get; set; } = new List<Course>();

        /// <summary>
        /// Average grade for the filtered results
        /// </summary>
        public double AverageGrade { get; set; }
    }

    /// <summary>
    /// View model for displaying trainee grades specifically.
    /// Used in the TraineeGrades view.
    /// </summary>
    public class TraineeGradesViewModel
    {
        /// <summary>
        /// The trainee whose grades are being displayed
        /// </summary>
        public User Trainee { get; set; } = null!;

        /// <summary>
        /// Collection of grades for the trainee
        /// </summary>
        public IEnumerable<Grade> Grades { get; set; } = new List<Grade>();

        /// <summary>
        /// Average grade for the trainee
        /// </summary>
        public double AverageGrade { get; set; }

        /// <summary>
        /// Total number of grades
        /// </summary>
        public int TotalGrades { get; set; }

        /// <summary>
        /// Highest grade achieved
        /// </summary>
        public int HighestGrade { get; set; }

        /// <summary>
        /// Lowest grade achieved
        /// </summary>
        public int LowestGrade { get; set; }
    }
}
