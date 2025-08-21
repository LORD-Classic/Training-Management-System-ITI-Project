using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Attributes
{
  /// <summary>
  /// Custom validation attribute that ensures email addresses are unique across the system.
  /// Works with ASP.NET Core Identity's ApplicationUser model.
  /// </summary>
  public class UniqueEmailAttribute : ValidationAttribute
  {
    /// <summary>
    /// Validates that the email address being entered doesn't already exist in the system.
    /// For edit operations, excludes the current user from the uniqueness check.
    /// </summary>
    /// <param name="value">The email address value to validate</param>
    /// <param name="validationContext">Context providing access to services and object instance</param>
    /// <returns>ValidationResult indicating success or failure with error message</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      // Allow null or empty values (handled by Required attribute if needed)
      if (value == null || string.IsNullOrEmpty(value.ToString()))
        return ValidationResult.Success;

      // Get user manager from dependency injection container
      var userManager = validationContext.GetService<UserManager<ApplicationUser>>();
      if (userManager == null)
        return ValidationResult.Success;

      var email = value.ToString()!;
      var currentUser = validationContext.ObjectInstance as ApplicationUser;

      // Check if a user with this email already exists
      var existingUser = userManager.Users
          .FirstOrDefault(u => u.Email!.ToLower() == email.ToLower());

      // If a user exists and it's not the current user being edited, return validation error
      if (existingUser != null && (currentUser == null || existingUser.Id != currentUser.Id))
      {
        return new ValidationResult("A user with this email address already exists.");
      }

      // Email is unique - validation passed
      return ValidationResult.Success;
    }
  }
}
