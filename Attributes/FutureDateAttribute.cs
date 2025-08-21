using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Attributes
{
  /// <summary>
  /// Custom validation attribute that ensures a DateTime property value is not in the past.
  /// This is used to validate session start dates and other future-oriented dates.
  /// Implements business rule: "Sessions cannot be scheduled for past dates"
  /// </summary>
  public class FutureDateAttribute : ValidationAttribute
  {
    /// <summary>
    /// Validates that the provided date value is today or in the future
    /// </summary>
    /// <param name="value">The DateTime value to validate</param>
    /// <returns>True if the date is today or in the future, false if it's in the past</returns>
    public override bool IsValid(object? value)
    {
      // Check if the value is a valid DateTime
      if (value is DateTime dateTime)
      {
        // Compare only the date part (ignore time) with today's date
        // This allows scheduling for today but prevents past dates
        return dateTime >= DateTime.Now.Date;
      }
      // Return false for null or non-DateTime values
      return false;
    }
  }
}
