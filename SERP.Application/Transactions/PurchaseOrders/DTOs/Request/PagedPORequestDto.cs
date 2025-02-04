using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class PagedPORequestDto: PagedRequestModel
    {
        public string branchPlantNo { get; set; }
        public bool includeClosed { get; set; }
    }
}
