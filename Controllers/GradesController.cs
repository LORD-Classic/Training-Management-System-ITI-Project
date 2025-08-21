using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Controllers
{
  /// <summary>
  /// Controller for managing grades and assessments.
  /// Requires authentication for all actions.
  /// </summary>
  [Authorize]
  public class GradesController : Controller
  {
    private readonly IGradeRepository _gradeRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;

    public GradesController(IGradeRepository gradeRepository, ISessionRepository sessionRepository, IUserRepository userRepository)
    {
      _gradeRepository = gradeRepository;
      _sessionRepository = sessionRepository;
      _userRepository = userRepository;
    }

    // GET: Grades
    public async Task<IActionResult> Index(int? filterByTraineeId, int? filterBySessionId)
    {
      var viewModel = new GradeListViewModel
      {
        FilterByTraineeId = filterByTraineeId,
        FilterBySessionId = filterBySessionId,
        AvailableTrainees = (await _userRepository.GetUsersByRoleAsync(UserRole.Trainee)).ToList(),
        AvailableSessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList()
      };

      if (filterByTraineeId.HasValue)
      {
        viewModel.Grades = (await _gradeRepository.GetGradesByTraineeAsync(filterByTraineeId.Value)).ToList();
      }
      else if (filterBySessionId.HasValue)
      {
        viewModel.Grades = (await _gradeRepository.GetGradesBySessionAsync(filterBySessionId.Value)).ToList();
      }
      else
      {
        viewModel.Grades = (await _gradeRepository.GetGradesWithDetailsAsync()).ToList();
      }

      return View(viewModel);
    }

    // GET: Grades/TraineeGrades/5
    public async Task<IActionResult> TraineeGrades(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var trainee = await _userRepository.GetByIdAsync(id.Value);
      if (trainee == null || trainee.Role != UserRole.Trainee)
      {
        return NotFound();
      }

      var viewModel = new TraineeGradesViewModel
      {
        Trainee = trainee,
        Grades = (await _gradeRepository.GetGradesByTraineeAsync(id.Value)).ToList()
      };

      return View(viewModel);
    }

    // GET: Grades/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var grade = await _gradeRepository.GetByIdAsync(id.Value);
      if (grade == null)
      {
        return NotFound();
      }

      return View(grade);
    }

    // GET: Grades/Create
    [InstructorOrAbove]
    public async Task<IActionResult> Create()
    {
      var viewModel = new GradeViewModel
      {
        AvailableSessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList(),
        AvailableTrainees = (await _userRepository.GetUsersByRoleAsync(UserRole.Trainee)).ToList()
      };
      return View(viewModel);
    }

    // POST: Grades/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [InstructorOrAbove]
    public async Task<IActionResult> Create(GradeViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        // Check if grade already exists for this trainee in this session
        var existingGrade = await _gradeRepository.GetGradeBySessionAndTraineeAsync(viewModel.SessionId, viewModel.TraineeId);
        if (existingGrade != null)
        {
          ModelState.AddModelError("", "A grade already exists for this trainee in this session.");
        }
        else
        {
          var grade = new Grade
          {
            SessionId = viewModel.SessionId,
            TraineeId = viewModel.TraineeId,
            Value = viewModel.Value
          };

          await _gradeRepository.AddAsync(grade);
          TempData["SuccessMessage"] = "Grade recorded successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      viewModel.AvailableSessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList();
      viewModel.AvailableTrainees = (await _userRepository.GetUsersByRoleAsync(UserRole.Trainee)).ToList();
      return View(viewModel);
    }

    // GET: Grades/Edit/5
    [InstructorOrAbove]
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var grade = await _gradeRepository.GetByIdAsync(id.Value);
      if (grade == null)
      {
        return NotFound();
      }

      var viewModel = new GradeViewModel
      {
        Id = grade.Id,
        SessionId = grade.SessionId,
        TraineeId = grade.TraineeId,
        Value = grade.Value,
        AvailableSessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList(),
        AvailableTrainees = (await _userRepository.GetUsersByRoleAsync(UserRole.Trainee)).ToList()
      };

      return View(viewModel);
    }

    // POST: Grades/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [InstructorOrAbove]
    public async Task<IActionResult> Edit(int id, GradeViewModel viewModel)
    {
      if (id != viewModel.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        var grade = await _gradeRepository.GetByIdAsync(id);
        if (grade == null)
        {
          return NotFound();
        }

        // Check if changing session/trainee would create duplicate
        if (grade.SessionId != viewModel.SessionId || grade.TraineeId != viewModel.TraineeId)
        {
          var existingGrade = await _gradeRepository.GetGradeBySessionAndTraineeAsync(viewModel.SessionId, viewModel.TraineeId);
          if (existingGrade != null && existingGrade.Id != grade.Id)
          {
            ModelState.AddModelError("", "A grade already exists for this trainee in this session.");
            viewModel.AvailableSessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList();
            viewModel.AvailableTrainees = (await _userRepository.GetUsersByRoleAsync(UserRole.Trainee)).ToList();
            return View(viewModel);
          }
        }

        grade.SessionId = viewModel.SessionId;
        grade.TraineeId = viewModel.TraineeId;
        grade.Value = viewModel.Value;

        await _gradeRepository.UpdateAsync(grade);
        TempData["SuccessMessage"] = "Grade updated successfully!";
        return RedirectToAction(nameof(Index));
      }

      viewModel.AvailableSessions = (await _sessionRepository.GetSessionsWithCourseAsync()).ToList();
      viewModel.AvailableTrainees = (await _userRepository.GetUsersByRoleAsync(UserRole.Trainee)).ToList();
      return View(viewModel);
    }

    // GET: Grades/Delete/5
    [AdminOrAbove]
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var grade = await _gradeRepository.GetByIdAsync(id.Value);
      if (grade == null)
      {
        return NotFound();
      }

      return View(grade);
    }

    // POST: Grades/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [AdminOrAbove]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var result = await _gradeRepository.DeleteAsync(id);
      if (result)
      {
        TempData["SuccessMessage"] = "Grade deleted successfully!";
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to delete grade.";
      }

      return RedirectToAction(nameof(Index));
    }
  }
}
