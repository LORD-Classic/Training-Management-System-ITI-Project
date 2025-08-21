using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Attributes
{
  /// <summary>
  /// Custom validation attribute that ensures course names are unique across the system.
  /// This implements the business rule requirement that no two courses can have the same name.
  /// Works for both creating new courses and editing existing ones.
  /// </summary>
  public class UniqueCourseNameAttribute : ValidationAttribute
  {
    /// <summary>
    /// Validates that the course name being entered doesn't already exist in the database.
    /// For edit operations, excludes the current course from the uniqueness check.
    /// </summary>
    /// <param name="value">The course name value to validate</param>
    /// <param name="validationContext">Context providing access to services and object instance</param>
    /// <returns>ValidationResult indicating success or failure with error message</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      // Allow null or empty values (handled by Required attribute if needed)
      if (value == null || string.IsNullOrEmpty(value.ToString()))
        return ValidationResult.Success;

      // Get database context from dependency injection container
      var context = validationContext.GetService<ApplicationDbContext>();
      if (context == null)
        return ValidationResult.Success;

      var courseName = value.ToString()!;
      var course = validationContext.ObjectInstance as Course;

      // Check if a course with this name already exists
      // For edit operations, exclude the current course from the check
      var existingCourse = context.Courses
          .FirstOrDefault(c => c.Name.ToLower() == courseName.ToLower() &&
                              (course == null || c.Id != course.Id));

      // If a duplicate course name is found, return validation error
      if (existingCourse != null)
      {
        return new ValidationResult("A course with this name already exists.");
      }

      // Course name is unique - validation passed
      return ValidationResult.Success;
    }
  }
}
