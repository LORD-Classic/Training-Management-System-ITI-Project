using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;

namespace Training_Management_System_ITI_Project.Controllers
{
    /// <summary>
    /// Controller for managing users in the training management system.
    /// Provides CRUD operations for user management.
    /// </summary>
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all users with optional filtering and search
        /// </summary>
        public async Task<IActionResult> Index(string searchTerm, UserRole? selectedRole)
        {
            try
            {
                var users = await _userRepository.SearchAsync(searchTerm ?? string.Empty);
                
                if (selectedRole.HasValue)
                {
                    users = users.Where(u => u.Role == selectedRole.Value);
                }

                var viewModel = new UserListViewModel
                {
                    Users = users,
                    SearchTerm = searchTerm ?? string.Empty,
                    SelectedRole = selectedRole,
                    TotalCount = users.Count()
                };

                ViewBag.Roles = GetRoleSelectList();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users");
                TempData["ErrorMessage"] = "An error occurred while retrieving users. Please try again.";
                return View(new UserListViewModel());
            }
        }

        /// <summary>
        /// Displays details of a specific user
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user details for ID: {UserId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving user details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays the form for creating a new user
        /// </summary>
        public IActionResult Create()
        {
            ViewBag.Roles = GetRoleSelectList();
            return View(new UserViewModel());
        }

        /// <summary>
        /// Handles the creation of a new user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if email is already in use
                    if (await _userRepository.IsEmailInUseAsync(viewModel.Email))
                    {
                        ModelState.AddModelError("Email", "This email address is already in use.");
                        ViewBag.Roles = GetRoleSelectList();
                        return View(viewModel);
                    }

                    var user = new User
                    {
                        Name = viewModel.Name,
                        Email = viewModel.Email,
                        Role = viewModel.Role
                    };

                    await _userRepository.AddAsync(user);
                    TempData["SuccessMessage"] = "User created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                ModelState.AddModelError("", "An error occurred while creating the user. Please try again.");
            }

            ViewBag.Roles = GetRoleSelectList();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the form for editing an existing user
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };

                ViewBag.Roles = GetRoleSelectList();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user for editing, ID: {UserId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the user. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the update of an existing user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
        {
            try
            {
                if (id != viewModel.Id)
                {
                    TempData["ErrorMessage"] = "Invalid user ID.";
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    // Check if email is already in use by another user
                    if (await _userRepository.IsEmailInUseAsync(viewModel.Email, id))
                    {
                        ModelState.AddModelError("Email", "This email address is already in use by another user.");
                        ViewBag.Roles = GetRoleSelectList();
                        return View(viewModel);
                    }

                    var user = await _userRepository.GetByIdAsync(id);
                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "User not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    user.Name = viewModel.Name;
                    user.Email = viewModel.Email;
                    user.Role = viewModel.Role;

                    await _userRepository.UpdateAsync(user);
                    TempData["SuccessMessage"] = "User updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user, ID: {UserId}", id);
                ModelState.AddModelError("", "An error occurred while updating the user. Please try again.");
            }

            ViewBag.Roles = GetRoleSelectList();
            return View(viewModel);
        }

        /// <summary>
        /// Displays the confirmation page for deleting a user
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user for deletion, ID: {UserId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the user. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Handles the deletion of a user
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _userRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "User deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user, ID: {UserId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the user. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Creates a select list for user roles
        /// </summary>
        private SelectList GetRoleSelectList()
        {
            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = UserRole.Trainee.ToString(), Text = "Trainee" },
                new SelectListItem { Value = UserRole.Instructor.ToString(), Text = "Instructor" },
                new SelectListItem { Value = UserRole.Admin.ToString(), Text = "Admin" }
            };

            return new SelectList(roles, "Value", "Text");
        }
    }
}
