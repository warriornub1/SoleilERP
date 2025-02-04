using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SERP.Domain.Common.Attributes
{
    public class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly int _precision;
        private readonly int _scale;

        public DecimalPrecisionAttribute(int precision, int scale)
        {
            _precision = precision;
            _scale = scale;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                var parts = value.ToString()!.Split('.');
                var integerPartLength = parts[0].Length;
                var fractionalPartLength = parts.Length > 1 ? parts[1].Length : 0;

                if (integerPartLength + fractionalPartLength <= _precision && fractionalPartLength <= _scale)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"The field {validationContext.DisplayName} must be a decimal with {_precision - _scale} digits before the decimal point and {_scale} digits after the decimal point.");
        }
    }
}
