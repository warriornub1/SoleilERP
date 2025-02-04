using Microsoft.AspNetCore.Http;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Transactions.PurchaseOrders.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.PurchaseOrders.Model
{
    public class CreatePORequestModel
    {
        [AcceptValue(Common.Constants.DomainConstant.Action.Draft,
            Common.Constants.DomainConstant.Action.Submit)]
        public string action { get; set; }

        public List<CreatePoInfoRequestModel> pos { get; set; }

        [StringLength(100)]
        public string? upload_source { get; set; }
        [MaxFileCount(10)]
        public List<IFormFile>? files { get; set; }
    }

    public class CreatePoInfoRequestModel
    {
        public CreatePOHeaderViewModel po_header { get; set; }
        public List<CreatePODetailViewModel>? po_detail { get; set; }
    }

    public class CreatePOHeaderViewModel : POHeaderViewModel
    {
        [Required]
        [AcceptValue(
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Draft,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.New)]
        public new string status_flag { get; set; }
    };

    public class CreatePODetailViewModel : PODetailViewModel
    {
        [Required]
        [AcceptValue(
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.Draft,
            Common.Constants.DomainConstant.PurchaseOrder.StatusFlag.New)]
        public new string status_flag { get; set; }
    };
}