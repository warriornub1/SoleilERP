using System.Reflection.Metadata.Ecma335;

namespace SERP.Application.Finance.Groups.DTOs.Response
{
    public class GroupTypeParentGroupModel
    {
        public List<ParentGrouping> items { get; set; }
    }

    public class ParentGrouping()
    {
        public int id { get; set; }
        public string group_code { get; set; }

        public string group_description { get; set; }
    }
}
