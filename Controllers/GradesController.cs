using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;

namespace Training_Management_System_ITI_Project.Controllers
{
    /// <summary>
    /// Controller for managing grades in the training management system.
    /// Provides CRUD operations for grade management.
    /// </summary>
    public class GradesController : Controller
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<GradesController> _logger;

        public GradesController(
            IGradeRepository gradeRepository, 
            ISessionRepository sessionRepository, 
            IUserRepository userRepository,
            ICourseRepository courseRepository,
            ILogger<GradesController> logger)
        {
            _gradeRepository = gradeRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all grades with optional filtering
        /// </summary>
        public async Task<IActionResult> Index(int? selectedSessionId, int? selectedTraineeId, int? selectedCourseId, int? minGrade, int? maxGrade)
        {
            try
            {
                var grades = await _gradeRepository.GetGradesWithDetailsAsync();

                // Apply filters
                if (selectedSessionId.HasValue)
                {
                    grades = grades.Where(g => g.SessionId == selectedSessionId.Value);
                }

                if (selectedTraineeId.HasValue)
                {
                    grades = grades.Where(g => g.TraineeId == selectedTraineeId.Value);
                }

                if (selectedCourseId.HasValue)
                {
                    grades = grades.Where(g => g.Session.CourseId == selectedCourseId.Value);
                }

                if (minGrade.HasValue)
                {
                    grades = grades.Where(g => g.Value >= minGrade.Value);
                }

                if (maxGrade.HasValue)
                {
                    grades = grades.Where(g => g.Value <= maxGrade.Value);
                }

                var availableSessions = await _sessionRepository.GetSessionsWithCourseAsync();
                var availableTrainees = await _userRepository.GetByRoleAsync(UserRole.Trainee);
                var availableCourses = await _courseRepository.GetCoursesWithInstructorAsync();

                var averageGrade = grades.Any() ? grades.Average(g => g.Value) : 0.0;

                var viewModel = new GradeListViewModel
                {
                    Grades = grades,
                    SelectedSessionId = selectedSessionId,
                    SelectedTraineeId = selectedTraineeId,
                    SelectedCourseId = selectedCourseId,
                    MinGrade = minGrade,
                    MaxGrade = maxGrade,
                    TotalCount = grades.Count(),
                    AvailableSessions = availableSessions,
                    AvailableTrainees = availableTrainees,
                    AvailableCourses = availableCourses,
                    AverageGrade = averageGrade
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving grades");
                TempData["ErrorMessage"] = "An error occurred while retrieving grades. Please try again.";
                return View(new GradeListViewModel());
            }
        }

        /// <summary>
        /// Displays details of a specific grade
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    TempData["ErrorMessage"] = "Grade not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving grade details for ID: {GradeId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving grade details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays the form for creating a new grade
        /// </summary>
        public async Task<IActionResult> Create()
        {
            try
            {
                var sessions = await _sessionRepository.GetSessionsWithCourseAsync();
                var trainees = await _userRepository.GetByRoleAsync(UserRole.Trainee);

                ViewBag.Sessions = new SelectList(sessions, "Id", "DisplayName");
                ViewBag.Trainees = new SelectList(trainees, "Id", "Name");

                return View(new GradeViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while preparing grade creation form");
                TempData["ErrorMessage"] = "An error occurred while preparing the form. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the creation of a new grade
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if grade already exists for this trainee in this session
                    if (await _gradeRepository.GradeExistsForTraineeInSessionAsync(viewModel.SessionId, viewModel.TraineeId))
                    {
                        ModelState.AddModelError("", "A grade already exists for this trainee in this session.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    var grade = new Grade
                    {
                        SessionId = viewModel.SessionId,
                        TraineeId = viewModel.TraineeId,
                        Value = viewModel.Value
                    };

                    await _gradeRepository.AddAsync(grade);
                    TempData["SuccessMessage"] = "Grade created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating grade");
                ModelState.AddModelError("", "An error occurred while creating the grade. Please try again.");
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the form for editing an existing grade
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    TempData["ErrorMessage"] = "Grade not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new GradeViewModel
                {
                    Id = grade.Id,
                    SessionId = grade.SessionId,
                    TraineeId = grade.TraineeId,
                    Value = grade.Value,
                    SessionName = grade.Session?.Course?.Name,
                    TraineeName = grade.Trainee?.Name
                };

                await PopulateViewBags();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving grade for editing, ID: {GradeId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the grade. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the update of an existing grade
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradeViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    TempData["ErrorMessage"] = "Invalid grade ID.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    // Check if grade already exists for this trainee in this session (excluding current grade)
                    if (await _gradeRepository.GradeExistsForTraineeInSessionAsync(viewModel.SessionId, viewModel.TraineeId, id))
                    {
                        ModelState.AddModelError("", "A grade already exists for this trainee in this session.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    var grade = await _gradeRepository.GetByIdAsync(id);
                    if (grade == null)
                    {
                        TempData["ErrorMessage"] = "Grade not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    grade.SessionId = viewModel.SessionId;
                    grade.TraineeId = viewModel.TraineeId;
                    grade.Value = viewModel.Value;

                    await _gradeRepository.UpdateAsync(grade);
                    TempData["SuccessMessage"] = "Grade updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating grade, ID: {GradeId}", id);
                ModelState.AddModelError("", "An error occurred while updating the grade. Please try again.");
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the confirmation page for deleting a grade
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    TempData["ErrorMessage"] = "Grade not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving grade for deletion, ID: {GradeId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the grade. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the deletion of a grade
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    TempData["ErrorMessage"] = "Grade not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _gradeRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Grade deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting grade, ID: {GradeId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the grade. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays grades for a specific trainee
        /// </summary>
        public async Task<IActionResult> TraineeGrades(int id)
        {
            try
            {
                var trainee = await _userRepository.GetByIdAsync(id);
                if (trainee == null)
                {
                    TempData["ErrorMessage"] = "Trainee not found.";
                    return RedirectToAction(nameof(Index));
                }

                var grades = await _gradeRepository.GetByTraineeAsync(id);
                var averageGrade = await _gradeRepository.GetAverageGradeForTraineeAsync(id);

                var viewModel = new TraineeGradesViewModel
                {
                    Trainee = trainee,
                    Grades = grades,
                    AverageGrade = averageGrade,
                    TotalGrades = grades.Count(),
                    HighestGrade = grades.Any() ? grades.Max(g => g.Value) : 0,
                    LowestGrade = grades.Any() ? grades.Min(g => g.Value) : 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving trainee grades for ID: {TraineeId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving trainee grades. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Populates ViewBag with necessary data for forms
        /// </summary>
        private async Task PopulateViewBags()
        {
            var sessions = await _sessionRepository.GetSessionsWithCourseAsync();
            var trainees = await _userRepository.GetByRoleAsync(UserRole.Trainee);

            ViewBag.Sessions = new SelectList(sessions, "Id", "DisplayName");
            ViewBag.Trainees = new SelectList(trainees, "Id", "Name");
        }
    }
}
