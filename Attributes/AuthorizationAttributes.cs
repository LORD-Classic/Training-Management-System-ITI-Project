using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Training_Management_System_ITI_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Training_Management_System_ITI_Project.Attributes
{
  /// <summary>
  /// Custom authorization attribute that enforces minimum role requirements.
  /// Implements hierarchical role checking where higher roles inherit lower role permissions.
  /// Role hierarchy: SuperAdmin > Admin > Instructor > Trainee
  /// </summary>
  public class MinimumRoleAttribute : Attribute, IAuthorizationFilter
  {
    private readonly UserRole _minimumRole;

    /// <summary>
    /// Initializes the attribute with the minimum required role
    /// </summary>
    /// <param name="minimumRole">The minimum role required to access the resource</param>
    public MinimumRoleAttribute(UserRole minimumRole)
    {
      _minimumRole = minimumRole;
    }

    /// <summary>
    /// Performs authorization check based on user's role hierarchy
    /// </summary>
    /// <param name="context">Authorization filter context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // Check if user is authenticated
      if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      // Get user manager from DI container
      var userManager = context.HttpContext.RequestServices
          .GetService<UserManager<ApplicationUser>>();

      if (userManager == null)
      {
        context.Result = new ForbidResult();
        return;
      }

      // Get current user
      var user = userManager.GetUserAsync(context.HttpContext.User).Result;
      if (user == null || !user.IsActive)
      {
        context.Result = new ForbidResult();
        return;
      }

      // Check if user's role meets minimum requirement
      if ((int)user.Role < (int)_minimumRole)
      {
        context.Result = new ForbidResult();
        return;
      }
    }
  }

  /// <summary>
  /// Authorization attribute that allows access only to specific roles (exact match)
  /// Unlike MinimumRole, this requires an exact role match
  /// </summary>
  public class RequireRoleAttribute : Attribute, IAuthorizationFilter
  {
    private readonly UserRole[] _allowedRoles;

    /// <summary>
    /// Initializes the attribute with allowed roles
    /// </summary>
    /// <param name="allowedRoles">Array of roles that are allowed access</param>
    public RequireRoleAttribute(params UserRole[] allowedRoles)
    {
      _allowedRoles = allowedRoles;
    }

    /// <summary>
    /// Performs authorization check for exact role match
    /// </summary>
    /// <param name="context">Authorization filter context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // Check if user is authenticated
      if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      // Get user manager from DI container
      var userManager = context.HttpContext.RequestServices
          .GetService<UserManager<ApplicationUser>>();

      if (userManager == null)
      {
        context.Result = new ForbidResult();
        return;
      }

      // Get current user
      var user = userManager.GetUserAsync(context.HttpContext.User).Result;
      if (user == null || !user.IsActive)
      {
        context.Result = new ForbidResult();
        return;
      }

      // Check if user's role is in allowed roles
      if (!_allowedRoles.Contains(user.Role))
      {
        context.Result = new ForbidResult();
        return;
      }
    }
  }

  /// <summary>
  /// Authorization attribute for SuperAdmin only access
  /// Convenience attribute for the highest privilege level
  /// </summary>
  public class SuperAdminOnlyAttribute : MinimumRoleAttribute
  {
    public SuperAdminOnlyAttribute() : base(UserRole.SuperAdmin)
    {
    }
  }

  /// <summary>
  /// Authorization attribute for Admin and above access
  /// Includes both Admin and SuperAdmin roles
  /// </summary>
  public class AdminOrAboveAttribute : MinimumRoleAttribute
  {
    public AdminOrAboveAttribute() : base(UserRole.Admin)
    {
    }
  }

  /// <summary>
  /// Authorization attribute for Instructor and above access
  /// Includes Instructor, Admin, and SuperAdmin roles
  /// </summary>
  public class InstructorOrAboveAttribute : MinimumRoleAttribute
  {
    public InstructorOrAboveAttribute() : base(UserRole.Instructor)
    {
    }
  }

  /// <summary>
  /// Authorization attribute that allows users to access only their own resources
  /// Admins and SuperAdmins can access any resource
  /// </summary>
  public class ResourceOwnerOrAdminAttribute : Attribute, IAuthorizationFilter
  {
    private readonly string _userIdParameterName;

    /// <summary>
    /// Initializes the attribute with the parameter name containing the user ID
    /// </summary>
    /// <param name="userIdParameterName">Name of the route/query parameter containing the user ID</param>
    public ResourceOwnerOrAdminAttribute(string userIdParameterName = "id")
    {
      _userIdParameterName = userIdParameterName;
    }

    /// <summary>
    /// Performs authorization check for resource ownership or admin privileges
    /// </summary>
    /// <param name="context">Authorization filter context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // Check if user is authenticated
      if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      // Get user manager from DI container
      var userManager = context.HttpContext.RequestServices
          .GetService<UserManager<ApplicationUser>>();

      if (userManager == null)
      {
        context.Result = new ForbidResult();
        return;
      }

      // Get current user
      var currentUser = userManager.GetUserAsync(context.HttpContext.User).Result;
      if (currentUser == null || !currentUser.IsActive)
      {
        context.Result = new ForbidResult();
        return;
      }

      // Admins and SuperAdmins can access any resource
      if (currentUser.Role >= UserRole.Admin)
      {
        return;
      }

      // Get the resource user ID from route parameters
      var resourceUserId = context.RouteData.Values[_userIdParameterName]?.ToString() ??
                         context.HttpContext.Request.Query[_userIdParameterName].FirstOrDefault();

      // Check if current user is the resource owner
      if (string.IsNullOrEmpty(resourceUserId) || currentUser.Id != resourceUserId)
      {
        context.Result = new ForbidResult();
        return;
      }
    }
  }
}
