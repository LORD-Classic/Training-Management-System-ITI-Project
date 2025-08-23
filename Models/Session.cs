using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Models
{
    /// <summary>
    /// Represents a training session for a specific course.
    /// Each session has a start and end date, and belongs to exactly one course.
    /// Sessions can have multiple trainees enrolled and grades recorded.
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
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        /// <summary>
        /// When the training session starts - must be in the future
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date & Time")]
        [FutureDate(ErrorMessage = "Start date must be in the future")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// When the training session ends - must be after start date
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date & Time")]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Session title or name - optional field for session identification
        /// </summary>
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [Display(Name = "Session Title")]
        public string? Title { get; set; }

        /// <summary>
        /// Session description - optional field for detailed session information
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        /// <summary>
        /// Maximum number of trainees allowed in this session
        /// </summary>
        [Range(1, 50, ErrorMessage = "Maximum trainees must be between 1 and 50")]
        [Display(Name = "Maximum Trainees")]
        public int? MaxTrainees { get; set; }

        /// <summary>
        /// Session location - optional field for physical or virtual location
        /// </summary>
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        /// <summary>
        /// Indicates whether the session is currently active and available for enrollment
        /// </summary>
        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When the session was created
        /// </summary>
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// When the session was last updated
        /// </summary>
        [Display(Name = "Last Updated")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties for Entity Framework relationships

        /// <summary>
        /// Navigation property to the associated Course entity
        /// </summary>
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;

        /// <summary>
        /// Collection of grades for trainees in this session
        /// </summary>
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

        /// <summary>
        /// Computed property for session duration
        /// </summary>
        [NotMapped]
        public TimeSpan Duration => EndDate - StartDate;

        /// <summary>
        /// Computed property to check if session is in the past
        /// </summary>
        [NotMapped]
        public bool IsPast => DateTime.Now > EndDate;

        /// <summary>
        /// Computed property to check if session is currently ongoing
        /// </summary>
        [NotMapped]
        public bool IsOngoing => DateTime.Now >= StartDate && DateTime.Now <= EndDate;

        /// <summary>
        /// Computed property to check if session is upcoming
        /// </summary>
        [NotMapped]
        public bool IsUpcoming => DateTime.Now < StartDate;
    }
}
