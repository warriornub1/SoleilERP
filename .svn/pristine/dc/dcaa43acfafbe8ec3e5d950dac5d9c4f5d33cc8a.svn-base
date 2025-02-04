using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class UpdateIntermediaryRequestDto
    {
        private string _action { get; set; }
        private string _status_flag { get; set; }
        [Required]
        [AcceptValue(DomainConstant.ActionFlag.Create, DomainConstant.ActionFlag.Update, DomainConstant.ActionFlag.Delete)]
        public string action
        {
            get => _action;
            set => _action = value.ToUpper();
        }
        [AcceptValue(DomainConstant.StatusFlag.Enabled, DomainConstant.StatusFlag.Disabled)]
        public string status_flag
        {
            get => _status_flag;
            set => _status_flag = value.ToUpper();
        }
        public int intermediary_supplier_id { get; set; }
        public int id { get; set; }
        public bool default_flag { get; set; }
    }
}
