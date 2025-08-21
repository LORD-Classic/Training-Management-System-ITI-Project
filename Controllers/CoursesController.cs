using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Controllers
{
  /// <summary>
  /// MVC Controller responsible for handling all course-related HTTP requests.
  /// Implements CRUD operations for courses with search functionality.
  /// Uses Repository Pattern for data access and ViewModels for data transfer.
  /// Requires authentication - all actions are protected.
  /// </summary>
  [Authorize]
  public class CoursesController : Controller
  {
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes the controller with required repository dependencies.
    /// Dependencies are injected by the ASP.NET Core DI container.
    /// </summary>
    /// <param name="courseRepository">Repository for course data operations</param>
    /// <param name="userRepository">Repository for user data operations (needed for instructor selection)</param>
    public CoursesController(ICourseRepository courseRepository, IUserRepository userRepository)
    {
      _courseRepository = courseRepository;
      _userRepository = userRepository;
    }

    /// <summary>
    /// Displays the course index page with optional search functionality.
    /// Supports searching by course name or category.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter courses</param>
    /// <returns>View with list of courses matching search criteria</returns>
    // GET: Courses
    public async Task<IActionResult> Index(string searchTerm)
    {
      // Create view model to pass data to the view
      var viewModel = new CourseSearchViewModel
      {
        SearchTerm = searchTerm
      };

      // Fetch courses based on search criteria
      if (string.IsNullOrEmpty(searchTerm))
      {
        // No search term provided - get all courses with instructor details
        viewModel.Courses = (await _courseRepository.GetCoursesWithInstructorAsync()).ToList();
      }
      else
      {
        // Search term provided - filter by name or category
        viewModel.Courses = (await _courseRepository.SearchByNameOrCategoryAsync(searchTerm)).ToList();
      }

      return View(viewModel);
    }

    /// <summary>
    /// Displays detailed information for a specific course.
    /// Shows course details including assigned instructor and related sessions.
    /// </summary>
    /// <param name="id">The unique identifier of the course to display</param>
    /// <returns>View with course details or NotFound if course doesn't exist</returns>
    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      // Validate that an ID was provided
      if (id == null)
      {
        return NotFound();
      }

      // Attempt to retrieve the course from the database
      var course = await _courseRepository.GetByIdAsync(id.Value);
      if (course == null)
      {
        return NotFound();
      }

      return View(course);
    }

    /// <summary>
    /// Displays the course creation form.
    /// Populates instructor dropdown with available instructors.
    /// Requires Instructor role or above.
    /// </summary>
    /// <returns>View with course creation form</returns>
    // GET: Courses/Create
    [InstructorOrAbove]
    public async Task<IActionResult> Create()
    {
      // Prepare view model with list of available instructors
      var viewModel = new CourseViewModel
      {
        AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList()
      };
      return View(viewModel);
    }

    // POST: Courses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [InstructorOrAbove]
    public async Task<IActionResult> Create(CourseViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        // Check if course name is unique
        if (!await _courseRepository.IsNameUniqueAsync(viewModel.Name))
        {
          ModelState.AddModelError("Name", "A course with this name already exists.");
        }
        else
        {
          var course = new Course
          {
            Name = viewModel.Name,
            Category = viewModel.Category,
            InstructorId = viewModel.InstructorId
          };

          await _courseRepository.AddAsync(course);
          TempData["SuccessMessage"] = "Course created successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      viewModel.AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList();
      return View(viewModel);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var course = await _courseRepository.GetByIdAsync(id.Value);
      if (course == null)
      {
        return NotFound();
      }

      var viewModel = new CourseViewModel
      {
        Id = course.Id,
        Name = course.Name,
        Category = course.Category,
        InstructorId = course.InstructorId,
        AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList()
      };

      return View(viewModel);
    }

    // POST: Courses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CourseViewModel viewModel)
    {
      if (id != viewModel.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        // Check if course name is unique (excluding current course)
        if (!await _courseRepository.IsNameUniqueAsync(viewModel.Name, viewModel.Id))
        {
          ModelState.AddModelError("Name", "A course with this name already exists.");
        }
        else
        {
          var course = await _courseRepository.GetByIdAsync(id);
          if (course == null)
          {
            return NotFound();
          }

          course.Name = viewModel.Name;
          course.Category = viewModel.Category;
          course.InstructorId = viewModel.InstructorId;

          await _courseRepository.UpdateAsync(course);
          TempData["SuccessMessage"] = "Course updated successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      viewModel.AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList();
      return View(viewModel);
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var course = await _courseRepository.GetByIdAsync(id.Value);
      if (course == null)
      {
        return NotFound();
      }

      return View(course);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var result = await _courseRepository.DeleteAsync(id);
      if (result)
      {
        TempData["SuccessMessage"] = "Course deleted successfully!";
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to delete course.";
      }

      return RedirectToAction(nameof(Index));
    }
  }
}
