using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Request
{
    public class PagedFilterIsRequestDto: SearchPagedRequestModel
    {
        //public string bpNo { get; set; }
        public HashSet<string>? incoterms { get; set; }

        public HashSet<int>? country_of_loading_list { get; set; }
        public HashSet<int>? port_of_loading_list { get; set; }

        public HashSet<int>? country_of_discharge_list { get; set; }
        public HashSet<int>? port_of_discharge_list { get; set; }

        public HashSet<string>? status_list { get; set; }

        public DateOnly? cargo_ready_date_from { get; set; }
        public DateOnly? cargo_ready_date_to { get; set; }
        public HashSet<int>? BranchPlants { get; set; }
        public HashSet<string>? inbound_shipment_request_group_no_list { get; set; }
    }
}
