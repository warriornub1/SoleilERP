using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model.Base;
using SERP.Domain.Transactions.InboundShipments.Model;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class UpdateASNRequestModel
    {
        [AcceptValue(DomainConstant.Action.Submit,
            DomainConstant.Action.Draft)]
        public string action { get; set; }

        public List<UpdateASNInfoModel> asns { get; set; }
    }

    public class UpdateASNInfoModel
    {
        public UpdateASNHeaderModel asn_header { get; set; }
        public List<UpdateASNDetailModel> asn_detail { get; set; }
        public UpdateShipmentInfoRequestModel shipment_info { get; set; }
    }

    public class UpdateASNHeaderModel : ASNHeaderViewModel
    {
        public int id { get; set; }
        [Required]
        [AcceptValue(
            DomainConstant.AdvancedShipmentNotices.StatusFlag.Draft,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.New,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.InProcess,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.Closed,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.Cancelled)]
        public new string status_flag { get; set; }
    }

    public class UpdateASNDetailModel : ASNDetailViewModel
    {
        public int id { get; set; }
        [Required]
        [AcceptValue(
            DomainConstant.AdvancedShipmentNotices.StatusFlag.Draft,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.New,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.InProcess,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.Closed,
            DomainConstant.AdvancedShipmentNotices.StatusFlag.Cancelled)]
        public new string status_flag { get; set; }
    }
}
