using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class UpdateItemMappingRequestDto
    {
        public int supplier_item_mapping_id { get; set; }
        public int item_id { get; set; }
        public string supplier_part_no { get; set; }
        public string supplier_material_code { get; set; }
        public string supplier_material_description { get; set; }
        public bool default_flag { get; set; }
        private string _action { get; set; }
        private string _status_flag { get; set; }
        [Required]
        [AcceptValue(ActionFlag.Create, ActionFlag.Update, ActionFlag.Delete)]
        public string action
        {
            get => _action;
            set => _action = value.ToUpper();
        }
        [AcceptValue(StatusFlag.Enabled, StatusFlag.Disabled)]
        public string status_flag
        {
            get => _status_flag;
            set => _status_flag = value.ToUpper();
        }
    }
}
