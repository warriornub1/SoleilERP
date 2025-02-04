namespace SERP.Application.Masters.Lovs.DTOs.Request
{
    public class LOVFilterRequestModel
    {
        public DateTime? create_date_to { get; set; }
        public DateTime? create_date_from { get; set; }
        public HashSet<LovTypeList>? lovTypeList { get; set; }
        public HashSet<StatusList>? statusList { get; set; }
        public bool? default_flag { get; set; }

    }

    public class LovTypeList
    {
        public string lov_type { get; set; }
    }

    public class StatusList
    {
        public string status { get; set; }
    }
}
