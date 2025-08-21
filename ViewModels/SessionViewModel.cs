using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Attributes;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        [FutureDate(ErrorMessage = "Start date cannot be in the past")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        public DateTime EndDate { get; set; }

        public string? CourseName { get; set; }
        public string? InstructorName { get; set; }
        public List<Course> AvailableCourses { get; set; } = new List<Course>();
    }

    public class SessionSearchViewModel
    {
        [Display(Name = "Search by Course Name")]
        public string? CourseNameSearch { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
