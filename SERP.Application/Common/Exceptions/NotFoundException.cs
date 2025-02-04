using FluentValidation.Results;
using System.Net;

namespace SERP.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public int StatusCode { get; }
        public int ErrorCode { get; }

        public NotFoundException(string message)
            : base(message)
        {
            ErrorCode = 0;
            StatusCode = (int)HttpStatusCode.NotFound;
        }

        public NotFoundException(int errorCode, string message)
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
            ErrorCode = errorCode;
        }
    }
}
