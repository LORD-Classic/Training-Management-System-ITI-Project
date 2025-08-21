using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;

namespace Training_Management_System_ITI_Project.Controllers
{
  /// <summary>
  /// Home controller that provides different experiences for authenticated and unauthenticated users
  /// </summary>
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ICourseRepository _courseRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger,
        ICourseRepository courseRepository,
        ISessionRepository sessionRepository,
        IUserRepository userRepository,
        IGradeRepository gradeRepository,
        UserManager<ApplicationUser> userManager)
    {
      _logger = logger;
      _courseRepository = courseRepository;
      _sessionRepository = sessionRepository;
      _userRepository = userRepository;
      _gradeRepository = gradeRepository;
      _userManager = userManager;
    }

    /// <summary>
    /// Main landing page - shows portfolio for unauthenticated users, dashboard for authenticated users
    /// </summary>
    public IActionResult Index()
    {
      // Check if user is authenticated
      if (User.Identity?.IsAuthenticated == true)
      {
        // Redirect authenticated users to dashboard
        return RedirectToAction("Dashboard");
      }

      // Show portfolio/landing page for unauthenticated users
      return View("Landing");
    }

    /// <summary>
    /// Dashboard for authenticated users only - shows system statistics and management options
    /// </summary>
    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
      var currentUser = await _userManager.GetUserAsync(User);

      // Get system statistics
      ViewBag.TotalCourses = (await _courseRepository.GetAllAsync()).Count();
      ViewBag.TotalSessions = (await _sessionRepository.GetAllAsync()).Count();
      ViewBag.TotalUsers = (await _userRepository.GetAllAsync()).Count();
      ViewBag.TotalGrades = (await _gradeRepository.GetAllAsync()).Count();

      // Pass user role for role-specific dashboard content
      ViewBag.UserRole = currentUser?.Role.ToString();
      ViewBag.UserName = currentUser?.FullName;

      return View();
    }

    /// <summary>
    /// Public action to allow student self-registration
    /// </summary>
    [AllowAnonymous]
    public IActionResult RegisterStudent()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
