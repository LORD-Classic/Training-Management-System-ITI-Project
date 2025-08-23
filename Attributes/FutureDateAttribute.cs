using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Attributes
{
    /// <summary>
    /// Custom validation attribute that ensures a date is in the future.
    /// Used for session start dates and other future-oriented date fields.
    /// </summary>
    public class FutureDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FutureDateAttribute
        /// </summary>
        public FutureDateAttribute() : base("Date must be in the future")
        {
        }

        /// <summary>
        /// Determines whether the value of the date is valid (in the future)
        /// </summary>
        /// <param name="value">The date value to validate</param>
        /// <returns>True if the date is in the future, false otherwise</returns>
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true; // Let Required attribute handle null values

            if (value is DateTime date)
            {
                return date > DateTime.Now;
            }

            return false;
        }

        /// <summary>
        /// Formats the error message with the display name
        /// </summary>
        /// <param name="name">The name of the field being validated</param>
        /// <returns>Formatted error message</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}
