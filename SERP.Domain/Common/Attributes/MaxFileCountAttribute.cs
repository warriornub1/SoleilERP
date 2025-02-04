using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Common.Attributes
{
    public class MaxFileCountAttribute : ValidationAttribute
    {
        private readonly int _maxCount;

        public MaxFileCountAttribute(int maxCount)
        {
            _maxCount = maxCount;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null && files.Count > _maxCount)
            {
                return new ValidationResult($"The maximum number of files allowed is {_maxCount}.");
            }

            return ValidationResult.Success;
        }
    }
}
