using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Instructor")]
        public int? InstructorId { get; set; }

        public string? InstructorName { get; set; }
        public List<User> AvailableInstructors { get; set; } = new List<User>();
    }

    public class CourseSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
