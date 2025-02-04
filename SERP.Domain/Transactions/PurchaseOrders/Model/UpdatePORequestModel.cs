using SERP.Domain.Common.Attributes;
using SERP.Domain.Transactions.PurchaseOrders.Model.Base;
using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common.Constants;

namespace SERP.Domain.Transactions.PurchaseOrders.Model
{
    public class UpdatePORequestModel
    {
        [AcceptValue(DomainConstant.Action.Submit,
            DomainConstant.Action.Draft)]
        public string action { get; set; }

        public List<UpdatePoInfoRequestModel> pos { get; set; }
    }

    public class UpdatePoInfoRequestModel
    {
        public UpdatePOHeaderViewModel po_header { get; set; }
        public List<UpdatePODetailViewModel> po_detail { get; set; }
    }

    public class UpdatePOHeaderViewModel : POHeaderViewModel
    {
        public int id { get; set; }
        [Obsolete("Not allow to update supplier", true)]
        public new int supplier_id { get; set; }
        [Required]
        [AcceptValue(
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Draft,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.New,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.InProcess,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Closed,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Cancelled)]
        public new string status_flag { get; set; }
    }

    public class UpdatePODetailViewModel : PODetailViewModel
    {
        public int id { get; set; }
        [Required]
        [AcceptValue(
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Draft,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.New,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.InProcess,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Closed,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Cancelled)]
        public new string status_flag { get; set; }
    }
}
