using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Controllers
{
  /// <summary>
  /// Controller for managing training sessions.
  /// Requires authentication for all actions.
  /// </summary>
  [Authorize]
  public class SessionsController : Controller
  {
    private readonly ISessionRepository _sessionRepository;
    private readonly ICourseRepository _courseRepository;

    public SessionsController(ISessionRepository sessionRepository, ICourseRepository courseRepository)
    {
      _sessionRepository = sessionRepository;
      _courseRepository = courseRepository;
    }

    // GET: Sessions
    public async Task<IActionResult> Index(string courseNameSearch)
    {
      var viewModel = new SessionSearchViewModel
      {
        CourseNameSearch = courseNameSearch
      };

      if (string.IsNullOrEmpty(courseNameSearch))
      {
        viewModel.Sessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList();
      }
      else
      {
        viewModel.Sessions = (await _sessionRepository.SearchByCourseNameAsync(courseNameSearch)).ToList();
      }

      return View(viewModel);
    }

    // GET: Sessions/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var session = await _sessionRepository.GetByIdAsync(id.Value);
      if (session == null)
      {
        return NotFound();
      }

      return View(session);
    }

    // GET: Sessions/Create
    [InstructorOrAbove]
    public async Task<IActionResult> Create()
    {
      var viewModel = new SessionViewModel
      {
        AvailableCourses = (await _courseRepository.GetCoursesWithInstructorAsync()).ToList(),
        StartDate = DateTime.Now.AddDays(1),
        EndDate = DateTime.Now.AddDays(30)
      };
      return View(viewModel);
    }

    // POST: Sessions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [InstructorOrAbove]
    public async Task<IActionResult> Create(SessionViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        var session = new Session
        {
          CourseId = viewModel.CourseId,
          StartDate = viewModel.StartDate,
          EndDate = viewModel.EndDate
        };

        await _sessionRepository.AddAsync(session);
        TempData["SuccessMessage"] = "Session created successfully!";
        return RedirectToAction(nameof(Index));
      }

      viewModel.AvailableCourses = (await _courseRepository.GetCoursesWithInstructorAsync()).ToList();
      return View(viewModel);
    }

    // GET: Sessions/Edit/5
    [InstructorOrAbove]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var session = await _sessionRepository.GetByIdAsync(id.Value);
      if (session == null)
      {
        return NotFound();
      }

      var viewModel = new SessionViewModel
      {
        Id = session.Id,
        CourseId = session.CourseId,
        StartDate = session.StartDate,
        EndDate = session.EndDate,
        AvailableCourses = (await _courseRepository.GetCoursesWithInstructorAsync()).ToList()
      };

      return View(viewModel);
    }

    // POST: Sessions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [InstructorOrAbove]
    public async Task<IActionResult> Edit(int id, SessionViewModel viewModel)
    {
      if (id != viewModel.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        var session = await _sessionRepository.GetByIdAsync(id);
        if (session == null)
        {
          return NotFound();
        }

        session.CourseId = viewModel.CourseId;
        session.StartDate = viewModel.StartDate;
        session.EndDate = viewModel.EndDate;

        await _sessionRepository.UpdateAsync(session);
        TempData["SuccessMessage"] = "Session updated successfully!";
        return RedirectToAction(nameof(Index));
      }

      viewModel.AvailableCourses = (await _courseRepository.GetCoursesWithInstructorAsync()).ToList();
      return View(viewModel);
    }

    // GET: Sessions/Delete/5
    [AdminOrAbove]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var session = await _sessionRepository.GetByIdAsync(id.Value);
      if (session == null)
      {
        return NotFound();
      }

      return View(session);
    }

    // POST: Sessions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [AdminOrAbove]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var result = await _sessionRepository.DeleteAsync(id);
      if (result)
      {
        TempData["SuccessMessage"] = "Session deleted successfully!";
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to delete session.";
      }

      return RedirectToAction(nameof(Index));
    }
  }
}
