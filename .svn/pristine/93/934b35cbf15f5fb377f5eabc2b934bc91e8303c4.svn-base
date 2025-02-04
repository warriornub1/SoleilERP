using SERP.Domain.Common.Attributes;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model.Base;
using SERP.Domain.Transactions.Containers.Model.Base;
using SERP.Domain.Transactions.InboundShipments.Model;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class CreateASNRequestModel
    {
        [AcceptValue(Common.Constants.DomainConstant.Action.Submit,
            Common.Constants.DomainConstant.Action.Draft)]
        public string action { get; set; }

        public List<CreateASNInfoModel> asns{ get; set; }
    }

    public class CreateASNInfoModel
    {
        public CreateASNHeaderModel asn_header { get; set; }
        public List<CreateASNDetailModel> asn_detail { get; set; }
        public CreateShipmentInfoRequestModel? shipment_info { get; set; }
        public List<ContainerRequestModel>? containers { get; set; }
    }

    public class CreateASNHeaderModel : ASNHeaderViewModel
    {
        [Required]
        [AcceptValue(Common.Constants.DomainConstant.AdvancedShipmentNotices.StatusFlag.Draft,
            Common.Constants.DomainConstant.AdvancedShipmentNotices.StatusFlag.New)]
        public new string status_flag { get; set; }
    }

    public class CreateASNDetailModel : ASNDetailViewModel
    {
        [Required]
        [AcceptValue(Common.Constants.DomainConstant.AdvancedShipmentNotices.StatusFlag.Draft,
            Common.Constants.DomainConstant.AdvancedShipmentNotices.StatusFlag.New)]
        public new string status_flag { get; set; }
    }
}
