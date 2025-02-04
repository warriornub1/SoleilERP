namespace SERP.Api.Common
{
    public class HttpContextService
    {
        private readonly HttpContextAccessor _httpContextAccessor;

        private const string UnknownUser = "unknown_user";
        class Claims
        {
            public const string UserId = "Sys_user_id";
            public const string BranchPlantId = "branch_plant_id";
        }
        class HeaderKey
        {
            public const string UserId = "Sys_user_id";
            public const string BranchPlantId = "branch_plant_id";
        }
        public HttpContextService()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public string GetCurrentUserId()
        {
            string userId = UnknownUser;

            if (_httpContextAccessor.HttpContext != null)
            {
                userId = GetClaimValue(Claims.UserId);

                if (!string.IsNullOrEmpty(userId)) return userId;

                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(HeaderKey.UserId, out var headerValue))
                {
                    userId = headerValue.FirstOrDefault() ?? UnknownUser;
                }
                else
                {
                    userId = UnknownUser;
                }
            }

            return userId;
        }
        public int GetCurrentBranchPlant()
        {
            string branchPlant = "3";

            if (_httpContextAccessor.HttpContext != null)
            {
                branchPlant = GetClaimValue(Claims.BranchPlantId);

                if (!string.IsNullOrEmpty(branchPlant)) return int.Parse(branchPlant);

                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(HeaderKey.BranchPlantId, out var headerValue))
                {
                    branchPlant = headerValue.FirstOrDefault() ?? "";
                }
                else
                {
                    branchPlant = "3";
                }
            }

            return int.Parse(branchPlant);
        }

        private string GetClaimValue(string type)
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == type)?.Value;
        }
    }
}
