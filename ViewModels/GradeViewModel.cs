using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.ViewModels
{
    public class GradeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Session is required")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Trainee is required")]
        [Display(Name = "Trainee")]
        public int TraineeId { get; set; }

        [Required(ErrorMessage = "Grade value is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade Value")]
        public int Value { get; set; }

        public string? SessionInfo { get; set; }
        public string? TraineeName { get; set; }
        public List<Session> AvailableSessions { get; set; } = new List<Session>();
        public List<User> AvailableTrainees { get; set; } = new List<User>();
    }

    public class GradeListViewModel
    {
        public List<Grade> Grades { get; set; } = new List<Grade>();
        public int? FilterByTraineeId { get; set; }
        public int? FilterBySessionId { get; set; }
        public List<User> AvailableTrainees { get; set; } = new List<User>();
        public List<Session> AvailableSessions { get; set; } = new List<Session>();
    }

    public class TraineeGradesViewModel
    {
        public User Trainee { get; set; } = null!;
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
