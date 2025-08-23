using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Training_Management_System_ITI_Project.Attributes
{
    /// <summary>
    /// Custom validation attribute that ensures one date is greater than another date property.
    /// Used for validating that end dates are after start dates.
    /// </summary>
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        /// <summary>
        /// Initializes a new instance of the DateGreaterThanAttribute
        /// </summary>
        /// <param name="otherPropertyName">The name of the other date property to compare against</param>
        public DateGreaterThanAttribute(string otherPropertyName) : base("Date must be after {0}")
        {
            _otherPropertyName = otherPropertyName;
        }

        /// <summary>
        /// Determines whether the value of the date is valid (greater than the other date)
        /// </summary>
        /// <param name="value">The date value to validate</param>
        /// <param name="validationContext">The validation context</param>
        /// <returns>ValidationResult indicating success or failure</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // Let Required attribute handle null values

            // Get the other property value
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
            {
                return new ValidationResult($"Unknown property: {_otherPropertyName}");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);
            if (otherValue == null)
                return ValidationResult.Success; // Let Required attribute handle null values

            // Ensure both values are DateTime
            if (value is DateTime currentDate && otherValue is DateTime otherDate)
            {
                if (currentDate <= otherDate)
                {
                    var displayName = otherProperty.GetCustomAttribute<DisplayAttribute>()?.Name ?? _otherPropertyName;
                    return new ValidationResult(string.Format(ErrorMessageString, displayName));
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Formats the error message with the display name
        /// </summary>
        /// <param name="name">The name of the field being validated</param>
        /// <returns>Formatted error message</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, _otherPropertyName);
        }
    }
}
