using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;

namespace Training_Management_System_ITI_Project.Controllers
{
    /// <summary>
    /// Controller for managing courses in the training management system.
    /// Provides CRUD operations for course management.
    /// </summary>
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseRepository courseRepository, IUserRepository userRepository, ILogger<CoursesController> logger)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all courses with optional filtering and search
        /// </summary>
        public async Task<IActionResult> Index(string searchTerm, string? selectedCategory)
        {
            try
            {
                var courses = await _courseRepository.SearchAsync(searchTerm ?? string.Empty);
                
                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    courses = courses.Where(c => c.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase));
                }

                var availableCategories = await _courseRepository.GetCategoriesAsync();

                var viewModel = new CourseListViewModel
                {
                    Courses = courses,
                    SearchTerm = searchTerm ?? string.Empty,
                    SelectedCategory = selectedCategory,
                    TotalCount = courses.Count(),
                    AvailableCategories = availableCategories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving courses");
                TempData["ErrorMessage"] = "An error occurred while retrieving courses. Please try again.";
                return View(new CourseListViewModel());
            }
        }

        /// <summary>
        /// Displays details of a specific course
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                {
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving course details for ID: {CourseId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving course details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays the form for creating a new course
        /// </summary>
        public async Task<IActionResult> Create()
        {
            try
            {
                var instructors = await _userRepository.GetByRoleAsync(UserRole.Instructor);
                ViewBag.Instructors = new SelectList(instructors, "Id", "Name");
                ViewBag.Categories = await GetCategorySelectList();
                
                return View(new CourseViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while preparing course creation form");
                TempData["ErrorMessage"] = "An error occurred while preparing the form. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the creation of a new course
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if course name is already in use
                    if (await _courseRepository.IsNameInUseAsync(viewModel.Name))
                    {
                        ModelState.AddModelError("Name", "This course name is already in use.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    var course = new Course
                    {
                        Name = viewModel.Name,
                        Category = viewModel.Category,
                        InstructorId = viewModel.InstructorId
                    };

                    await _courseRepository.AddAsync(course);
                    TempData["SuccessMessage"] = "Course created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating course");
                ModelState.AddModelError("", "An error occurred while creating the course. Please try again.");
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the form for editing an existing course
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                {
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new CourseViewModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    Category = course.Category,
                    InstructorId = course.InstructorId,
                    InstructorName = course.Instructor?.Name
                };

                await PopulateViewBags();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving course for editing, ID: {CourseId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the update of an existing course
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    TempData["ErrorMessage"] = "Invalid course ID.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    // Check if course name is already in use by another course
                    if (await _courseRepository.IsNameInUseAsync(viewModel.Name, id))
                    {
                        ModelState.AddModelError("Name", "This course name is already in use by another course.");
                        await PopulateViewBags();
                        return View(viewModel);
                    }

                    var course = await _courseRepository.GetByIdAsync(id);
                    if (course == null)
                    {
                        TempData["ErrorMessage"] = "Course not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    course.Name = viewModel.Name;
                    course.Category = viewModel.Category;
                    course.InstructorId = viewModel.InstructorId;

                    await _courseRepository.UpdateAsync(course);
                    TempData["SuccessMessage"] = "Course updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating course, ID: {CourseId}", id);
                ModelState.AddModelError("", "An error occurred while updating the course. Please try again.");
            }

            await PopulateViewBags();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the confirmation page for deleting a course
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                {
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving course for deletion, ID: {CourseId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the deletion of a course
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id);
                if (course == null)
                {
                    TempData["ErrorMessage"] = "Course not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _courseRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "Course deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting course, ID: {CourseId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Populates ViewBag with necessary data for forms
        /// </summary>
        private async Task PopulateViewBags()
        {
            var instructors = await _userRepository.GetByRoleAsync(UserRole.Instructor);
            ViewBag.Instructors = new SelectList(instructors, "Id", "Name");
            ViewBag.Categories = await GetCategorySelectList();
        }

        /// <summary>
        /// Creates a select list for course categories
        /// </summary>
        private async Task<SelectList> GetCategorySelectList()
        {
            var categories = await _courseRepository.GetCategoriesAsync();
            var categoryList = categories.ToList();
            
            // Add common categories if none exist
            if (!categoryList.Any())
            {
                categoryList = new List<string> { "Programming", "Design", "Management", "Marketing", "Finance", "Other" };
            }

            return new SelectList(categoryList);
        }
    }
}
