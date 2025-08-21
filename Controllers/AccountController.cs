using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.ViewModels;

namespace Training_Management_System_ITI_Project.Controllers
{
  /// <summary>
  /// Controller responsible for authentication operations including login, logout, and registration.
  /// Implements ASP.NET Core Identity for secure user authentication and role-based authorization.
  /// </summary>
  public class AccountController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    /// <summary>
    /// Initializes the AccountController with required Identity services
    /// </summary>
    /// <param name="userManager">Identity user manager for user operations</param>
    /// <param name="signInManager">Identity sign-in manager for authentication</param>
    /// <param name="logger">Logger for recording authentication events</param>
    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
    }

    /// <summary>
    /// Displays the login page
    /// </summary>
    /// <param name="returnUrl">URL to redirect to after successful login</param>
    /// <returns>Login view</returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
      var model = new LoginViewModel
      {
        ReturnUrl = returnUrl
      };
      return View(model);
    }

    /// <summary>
    /// Processes user login attempt
    /// </summary>
    /// <param name="model">Login credentials and options</param>
    /// <returns>Redirect to return URL on success, or login view with errors on failure</returns>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      // Attempt to sign in the user
      var result = await _signInManager.PasswordSignInAsync(
          model.Email,
          model.Password,
          model.RememberMe,
          lockoutOnFailure: true);

      if (result.Succeeded)
      {
        _logger.LogInformation("User {Email} logged in successfully", model.Email);

        // Redirect to return URL or default page
        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
          return Redirect(model.ReturnUrl);
        }
        return RedirectToAction("Index", "Home");
      }

      if (result.IsLockedOut)
      {
        _logger.LogWarning("User {Email} account locked out", model.Email);
        ModelState.AddModelError(string.Empty, "Your account has been locked out due to multiple failed login attempts.");
      }
      else
      {
        _logger.LogWarning("Invalid login attempt for {Email}", model.Email);
        ModelState.AddModelError(string.Empty, "Invalid email or password.");
      }

      return View(model);
    }

    /// <summary>
    /// Displays the registration page
    /// Only accessible by Admin and SuperAdmin users
    /// </summary>
    /// <returns>Registration view</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult Register()
    {
      return View(new RegisterViewModel());
    }

    /// <summary>
    /// Processes user registration
    /// Creates new user accounts with role-based restrictions
    /// </summary>
    /// <param name="model">Registration information</param>
    /// <returns>Success page or registration view with errors</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      // Get current user to check permissions
      var currentUser = await _userManager.GetUserAsync(User);
      if (currentUser == null)
      {
        return RedirectToAction("Login");
      }

      // Only SuperAdmin can create Admin accounts
      if (model.Role == UserRole.Admin && currentUser.Role != UserRole.SuperAdmin)
      {
        ModelState.AddModelError(string.Empty, "Only Super Administrators can create Admin accounts.");
        return View(model);
      }

      // Only SuperAdmin can create other SuperAdmin accounts
      if (model.Role == UserRole.SuperAdmin && currentUser.Role != UserRole.SuperAdmin)
      {
        ModelState.AddModelError(string.Empty, "Only Super Administrators can create Super Admin accounts.");
        return View(model);
      }

      // Create new user
      var user = new ApplicationUser
      {
        UserName = model.Email,
        Email = model.Email,
        FullName = model.FullName,
        Role = model.Role,
        EmailConfirmed = true // Auto-confirm for internal registration
      };

      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {
        _logger.LogInformation("User {Email} created successfully with role {Role}", model.Email, model.Role);

        // Add user to appropriate role
        await _userManager.AddToRoleAsync(user, model.Role.ToString());

        TempData["SuccessMessage"] = $"User {model.FullName} has been created successfully.";
        return RedirectToAction("ManageUsers");
      }

      // Add Identity errors to ModelState
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View(model);
    }

    /// <summary>
    /// Public student registration - allows anyone to register as a trainee/student
    /// </summary>
    /// <returns>Student registration view</returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegisterStudent()
    {
      return View(new RegisterViewModel { Role = UserRole.Trainee });
    }

    /// <summary>
    /// Processes public student registration
    /// </summary>
    /// <param name="model">Student registration information</param>
    /// <returns>Success page or registration view with errors</returns>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterStudent(RegisterViewModel model)
    {
      // Force role to be Trainee for public registration
      model.Role = UserRole.Trainee;

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      // Create new student user
      var user = new ApplicationUser
      {
        UserName = model.Email,
        Email = model.Email,
        FullName = model.FullName,
        Role = UserRole.Trainee, // Always trainee for public registration
        EmailConfirmed = true // Auto-confirm for student registration
      };

      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {
        _logger.LogInformation("Student {Email} registered successfully", model.Email);

        // Add user to Trainee role
        await _userManager.AddToRoleAsync(user, UserRole.Trainee.ToString());

        // Auto-login the new student
        await _signInManager.SignInAsync(user, isPersistent: false);

        TempData["SuccessMessage"] = $"Welcome {model.FullName}! Your student account has been created successfully.";
        return RedirectToAction("Dashboard", "Home");
      }

      // Add Identity errors to ModelState
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View(model);
    }

    /// <summary>
    /// Logs out the current user
    /// </summary>
    /// <returns>Redirect to home page</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      _logger.LogInformation("User logged out");
      return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Displays user management page for administrators
    /// Shows list of users with filtering options
    /// </summary>
    /// <param name="searchTerm">Search term for user names or emails</param>
    /// <param name="roleFilter">Filter by user role</param>
    /// <param name="isActiveFilter">Filter by active status</param>
    /// <returns>User management view</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> ManageUsers(string? searchTerm, UserRole? roleFilter, bool? isActiveFilter)
    {
      var currentUser = await _userManager.GetUserAsync(User);
      var users = _userManager.Users.AsQueryable();

      // Apply search filter
      if (!string.IsNullOrEmpty(searchTerm))
      {
        users = users.Where(u => u.FullName.Contains(searchTerm) || u.Email!.Contains(searchTerm));
      }

      // Apply role filter
      if (roleFilter.HasValue)
      {
        users = users.Where(u => u.Role == roleFilter.Value);
      }

      // Apply active status filter
      if (isActiveFilter.HasValue)
      {
        users = users.Where(u => u.IsActive == isActiveFilter.Value);
      }

      // Non-SuperAdmins cannot see SuperAdmins
      if (currentUser?.Role != UserRole.SuperAdmin)
      {
        users = users.Where(u => u.Role != UserRole.SuperAdmin);
      }

      var userList = users.Select(u => new UserProfileViewModel
      {
        Id = u.Id,
        FullName = u.FullName,
        Email = u.Email!,
        Role = u.Role,
        IsActive = u.IsActive,
        CreatedAt = u.CreatedAt,
        UpdatedAt = u.UpdatedAt
      }).ToList();

      var viewModel = new ManageUsersViewModel
      {
        Users = userList,
        SearchTerm = searchTerm,
        RoleFilter = roleFilter,
        IsActiveFilter = isActiveFilter
      };

      return View(viewModel);
    }

    /// <summary>
    /// Toggles user active status (activate/deactivate)
    /// </summary>
    /// <param name="userId">ID of the user to toggle</param>
    /// <returns>Redirect to user management</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleUserStatus(string userId)
    {
      var currentUser = await _userManager.GetUserAsync(User);
      var targetUser = await _userManager.FindByIdAsync(userId);

      if (targetUser == null)
      {
        TempData["ErrorMessage"] = "User not found.";
        return RedirectToAction("ManageUsers");
      }

      // Prevent non-SuperAdmins from modifying SuperAdmin accounts
      if (targetUser.Role == UserRole.SuperAdmin && currentUser?.Role != UserRole.SuperAdmin)
      {
        TempData["ErrorMessage"] = "You don't have permission to modify Super Administrator accounts.";
        return RedirectToAction("ManageUsers");
      }

      // Prevent users from deactivating themselves
      if (targetUser.Id == currentUser?.Id)
      {
        TempData["ErrorMessage"] = "You cannot deactivate your own account.";
        return RedirectToAction("ManageUsers");
      }

      targetUser.IsActive = !targetUser.IsActive;
      targetUser.UpdatedAt = DateTime.UtcNow;

      var result = await _userManager.UpdateAsync(targetUser);

      if (result.Succeeded)
      {
        var status = targetUser.IsActive ? "activated" : "deactivated";
        TempData["SuccessMessage"] = $"User {targetUser.FullName} has been {status}.";
        _logger.LogInformation("User {UserId} {Status} by {CurrentUserId}",
            targetUser.Id, status, currentUser?.Id);
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to update user status.";
      }

      return RedirectToAction("ManageUsers");
    }

    /// <summary>
    /// Displays access denied page
    /// </summary>
    /// <returns>Access denied view</returns>
    [HttpGet]
    public IActionResult AccessDenied()
    {
      return View();
    }

    /// <summary>
    /// Displays user profile page
    /// </summary>
    /// <returns>Profile view</returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return RedirectToAction("Login");
      }

      var model = new UserProfileViewModel
      {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email!,
        Role = user.Role,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
      };

      return View(model);
    }

    /// <summary>
    /// Displays change password page
    /// </summary>
    /// <returns>Change password view</returns>
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
      return View(new ChangePasswordViewModel());
    }

    /// <summary>
    /// Processes password change request
    /// </summary>
    /// <param name="model">Password change information</param>
    /// <returns>Success page or change password view with errors</returns>
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return RedirectToAction("Login");
      }

      var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

      if (result.Succeeded)
      {
        await _signInManager.RefreshSignInAsync(user);
        TempData["SuccessMessage"] = "Your password has been changed successfully.";
        _logger.LogInformation("User {UserId} changed their password", user.Id);
        return RedirectToAction("Profile");
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return View(model);
    }
  }
}
