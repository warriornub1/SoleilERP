using Newtonsoft.Json;

namespace SERP.Application.Masters.ApplicationTokens
{
    public class Token
    {
        public string email { get; set; }
        public string idpreferenceid { get; set; }
        public string username { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }

        [JsonProperty(".issued")]
        public DateTime issued { get; set; }

        [JsonProperty(".expires")]
        public DateTime expires { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
