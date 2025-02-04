using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.InboundShipments.Model.Base
{
    public class ShipmentInfoRequestModel
    {
        [StringLength(50)]
        public string freight_method { get; set; }
        [StringLength(50)]
        public string incoterm { get; set; }
        public DateOnly? cargo_ready_date { get; set; }
        public DateOnly? port_of_loading_etd { get; set; }
        public DateOnly? port_of_discharge_eta { get; set; }
        [StringLength(255)]
        public string cargo_description { get; set; }
        public int? country_of_loading_id { get; set; }
        public int? port_of_loading_id { get; set; }
        public int? country_of_discharge_id { get; set; }
        public int? port_of_discharge_id { get; set; }
        public int? bl_awb_total_packages { get; set; }
        public decimal? bl_awb_total_gross_weight { get; set; }
        public decimal? bl_awb_volume { get; set; }
        public string? bl_awb_no { get; set; }
        public int? bl_awb_id { get; set; }
        [StringLength(50)]
        public string? vessel_flight_no { get; set; }
        [StringLength(50)]
        public string? connecting_vessel_flight_no { get; set; }
    }
}
