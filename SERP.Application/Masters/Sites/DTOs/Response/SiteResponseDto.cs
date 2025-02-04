namespace SERP.Application.Masters.Sites.DTOs.Response
{
    public class SiteResponseDto
    {
        public int id { get; set; }

        /// <summary>
        /// Site No.
        /// </summary>
        public string? site_no { get; set; }

        /// <summary>
        /// Site Name
        /// </summary>
        public string? site_name { get; set; }

        /// <summary>
        /// Address Line 1
        /// </summary>
        public string? address_line_1 { get; set; }

        /// <summary>
        /// Address Line 2
        /// </summary>
        public string? address_line_2 { get; set; }

        /// <summary>
        /// Address Line 3
        /// </summary>
        public string? address_line_3 { get; set; }

        /// <summary>
        /// Address Line 4
        /// </summary>
        public string? address_line_4 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string? city { get; set; }

        /// <summary>
        /// State/Province
        /// </summary>
        public string? state_province { get; set; }

        /// <summary>
        /// Postal Code
        /// </summary>
        public string? postal_code { get; set; }

        /// <summary>
        /// Country code (two-letter ISO code)
        /// </summary>
        public string? country { get; set; }

        /// <summary>
        /// County
        /// </summary>
        public string? county { get; set; }
    }
}
