using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.ViewModels;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Controllers
{
  /// <summary>
  /// Controller for managing users and user roles.
  /// Requires authentication for all actions.
  /// </summary>
  [Authorize]
  [AdminOrAbove] // User management should be restricted to Admin level and above
  public class UsersController : Controller
  {
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    // GET: Users
    public async Task<IActionResult> Index(UserRole? filterByRole)
    {
      var viewModel = new UserListViewModel
      {
        FilterByRole = filterByRole
      };

      if (filterByRole.HasValue)
      {
        viewModel.Users = (await _userRepository.GetUsersByRoleAsync(filterByRole.Value)).ToList();
      }
      else
      {
        viewModel.Users = (await _userRepository.GetAllAsync()).ToList();
      }

      return View(viewModel);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _userRepository.GetByIdAsync(id.Value);
      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
      var viewModel = new UserViewModel();
      return View(viewModel);
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        // Check if email is unique
        if (!await _userRepository.IsEmailUniqueAsync(viewModel.Email))
        {
          ModelState.AddModelError("Email", "A user with this email already exists.");
        }
        else
        {
          var user = new User
          {
            Name = viewModel.Name,
            Email = viewModel.Email,
            Role = viewModel.Role
          };

          await _userRepository.AddAsync(user);
          TempData["SuccessMessage"] = "User created successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      return View(viewModel);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _userRepository.GetByIdAsync(id.Value);
      if (user == null)
      {
        return NotFound();
      }

      var viewModel = new UserViewModel
      {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role
      };

      return View(viewModel);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
    {
      if (id != viewModel.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        // Check if email is unique (excluding current user)
        if (!await _userRepository.IsEmailUniqueAsync(viewModel.Email, viewModel.Id))
        {
          ModelState.AddModelError("Email", "A user with this email already exists.");
        }
        else
        {
          var user = await _userRepository.GetByIdAsync(id);
          if (user == null)
          {
            return NotFound();
          }

          user.Name = viewModel.Name;
          user.Email = viewModel.Email;
          user.Role = viewModel.Role;

          await _userRepository.UpdateAsync(user);
          TempData["SuccessMessage"] = "User updated successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      return View(viewModel);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _userRepository.GetByIdAsync(id.Value);
      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var result = await _userRepository.DeleteAsync(id);
      if (result)
      {
        TempData["SuccessMessage"] = "User deleted successfully!";
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to delete user.";
      }

      return RedirectToAction(nameof(Index));
    }
  }
}
