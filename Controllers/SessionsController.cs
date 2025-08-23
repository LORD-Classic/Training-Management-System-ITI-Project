using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;

namespace Training_Management_System_ITI_Project.Controllers
{
    /// <summary>
    /// Controller for managing sessions in the training management system.
    /// Provides CRUD operations for session management.
    /// </summary>
    public class SessionsController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<SessionsController> _logger;

        public SessionsController(ISessionRepository sessionRepository, ICourseRepository courseRepository, ILogger<SessionsController> logger)
        {
            _sessionRepository = sessionRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all sessions with optional filtering and search
        /// </summary>
        public async Task<IActionResult> Index(string searchTerm, int? selectedCourseId, DateTime? selectedDate)
        {
            try
            {
                var sessions = await _sessionRepository.SearchByCourseNameAsync(searchTerm ?? string.Empty);
                
                if (selectedCourseId.HasValue)
                {
                    sessions = sessions.Where(s => s.CourseId == selectedCourseId.Value);
                }

                if (selectedDate.HasValue)
                {
                    sessions = sessions.Where(s => s.StartDate.Date == selectedDate.Value.Date);
                }

                var availableCourses = await _courseRepository.GetCoursesWithInstructorAsync();

                var viewModel = new SessionListViewModel
                {
                    Sessions = sessions,
                    SearchTerm = searchTerm ?? string.Empty,
                    SelectedCourseId = selectedCourseId,
                    SelectedDate = selectedDate,
                    TotalCount = sessions.Count(),
                    AvailableCourses = availableCourses
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sessions");
                TempData["ErrorMessage"] = "An error occurred while retrieving sessions. Please try again.";
                return View(new SessionListViewModel());
            }
        }

        /// <summary>
        /// Displays details of a specific session
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    TempData["ErrorMessage"] = "Session not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving session details for ID: {SessionId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving session details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays the form for creating a new session
        /// </summary>
        public async Task<IActionResult> Create()
        {
            try
            {
                var courses = await _courseRepository.GetCoursesWithInstructorAsync();
                ViewBag.Courses = new SelectList(courses, "Id", "Name");
                
                var viewModel = new SessionViewModel
                {
                    StartDate = DateTime.Now.AddDays(1).AddHours(9), // Default to tomorrow at 9 AM
                    EndDate = DateTime.Now.AddDays(1).AddHours(17)   // Default to tomorrow at 5 PM
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while preparing session creation form");
                TempData["ErrorMessage"] = "An error occurred while preparing the form. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the creation of a new session
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SessionViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Validate business rules
                    if (viewModel.StartDate < DateTime.Now)
                    {
                        ModelState.AddModelError("StartDate", "Start date cannot be in the past.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    if (viewModel.EndDate <= viewModel.StartDate)
                    {
                        ModelState.AddModelError("EndDate", "End date must be after start date.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    var session = new Session
                    {
                        CourseId = viewModel.CourseId,
                        StartDate = viewModel.StartDate,
                        EndDate = viewModel.EndDate
                    };

                    await _sessionRepository.AddAsync(session);
                    TempData["SuccessMessage"] = "Session created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating session");
                ModelState.AddModelError("", "An error occurred while creating the session. Please try again.");
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the form for editing an existing session
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    TempData["ErrorMessage"] = "Session not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new SessionViewModel
                {
                    Id = session.Id,
                    CourseId = session.CourseId,
                    CourseName = session.Course?.Name,
                    StartDate = session.StartDate,
                    EndDate = session.EndDate
                };

                await PopulateViewBags();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving session for editing, ID: {SessionId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the session. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the update of an existing session
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SessionViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    TempData["ErrorMessage"] = "Invalid session ID.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    // Validate business rules
                    if (viewModel.StartDate < DateTime.Now)
                    {
                        ModelState.AddModelError("StartDate", "Start date cannot be in the past.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    if (viewModel.EndDate <= viewModel.StartDate)
                    {
                        ModelState.AddModelError("EndDate", "End date must be after start date.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    var session = await _sessionRepository.GetByIdAsync(id);
                    if (session == null)
                    {
                        TempData["ErrorMessage"] = "Session not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    session.CourseId = viewModel.CourseId;
                    session.StartDate = viewModel.StartDate;
                    session.EndDate = viewModel.EndDate;

                    await _sessionRepository.UpdateAsync(session);
                    TempData["SuccessMessage"] = "Session updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating session, ID: {SessionId}", id);
                ModelState.AddModelError("", "An error occurred while updating the session. Please try again.");
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the confirmation page for deleting a session
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    TempData["ErrorMessage"] = "Session not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving session for deletion, ID: {SessionId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the session. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the deletion of a session
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var session = await _sessionRepository.GetByIdAsync(id);
                if (session == null)
                {
                    TempData["ErrorMessage"] = "Session not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _sessionRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Session deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting session, ID: {SessionId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the session. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Populates ViewBag with necessary data for forms
        /// </summary>
        private async Task PopulateViewBags()
        {
            var courses = await _courseRepository.GetCoursesWithInstructorAsync();
            ViewBag.Courses = new SelectList(courses, "Id", "Name");
        }
    }
}
