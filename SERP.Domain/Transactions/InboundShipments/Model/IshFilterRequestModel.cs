namespace SERP.Domain.Transactions.InboundShipments.Model
{
    public class IshFilterRequestModel
    {
        public HashSet<string>? incoterms { get; set; }

        public HashSet<int>? country_of_loading_list { get; set; }
        public HashSet<int>? port_of_loading_list { get; set; }

        public HashSet<int>? country_of_discharge_list { get; set; }
        public HashSet<int>? port_of_discharge_list { get; set; }

        public HashSet<string>? status_list { get; set; }
        public HashSet<int>? BranchPlants { get; set; }
    }
}
