using FluentValidation.Results;

namespace SERP.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public int ErrorCode { get; }
        public object ErrorDetails { get; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }

        public BadRequestException(string message)
            : base(message)
        {
            ErrorCode = 0;
            ErrorDetails = new object();
        }

        public BadRequestException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public BadRequestException(int errorCode, string message, object details)
            : base(message)
        {
            ErrorCode = errorCode;
            ErrorDetails = details;
        }

        public BadRequestException(ValidationResult validationResult)
        {
            ValidationErrors = validationResult.ToDictionary();
        }
    }
}
