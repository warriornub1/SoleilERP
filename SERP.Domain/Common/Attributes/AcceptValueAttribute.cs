using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AcceptValueAttribute : ValidationAttribute
    {
        private readonly HashSet<string> _validFlags;

        public AcceptValueAttribute(params string[] validFlags)
        {
            _validFlags = new HashSet<string>(validFlags);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is string actionFlag && _validFlags.Contains(actionFlag))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"This field must be one of: {string.Join(", ", _validFlags)}");
        }
    }
}
