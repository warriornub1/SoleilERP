using System.Net;

namespace SERP.Api.Common.APIMessages
{
    public class APIResponse
    {
        public bool isSuccess { get; set; } = false;
        public object result { get; set; }
        public List<APIError> errors { get; set; } = new();
        public List<APIWaring> warnings { get; set; } = new();

        public void AddError(int errorCode, string errorMessage)
        {
            APIError error = new APIError(errorCode, errorMessage);
            errors.Add(error);
        }

        public void AddWarning(string warningMessage)
        {
            APIWaring warning = new APIWaring(warningMessage);
            warnings.Add(warning);
        }

    }
}
