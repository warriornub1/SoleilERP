using Microsoft.AspNetCore.Http;
using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Request
{
    public class CreateInboundShipmentRequestDto
    {
        public int branch_plant_id { get; set; }
        [StringLength(50)]
        public string freight_method { get; set; }
        [StringLength(50)]
        public string incoterm { get; set; }
        public DateOnly? cargo_ready_date { get; set; }
        public DateOnly? port_of_loading_etd { get; set; }
        public DateOnly? port_of_discharge_eta { get; set; }
        public int country_of_loading_id { get; set; }
        public int port_of_loading_id { get; set; }
        public int country_of_discharge_id { get; set; }
        public int port_of_discharge_id { get; set; }
        [StringLength(50)]
        public string? vessel_flight_no { get; set; }
        [StringLength(50)]
        public string? connecting_vessel_flight_no { get; set; }
        public DateOnly? notice_of_arrival_date { get; set; }
        [StringLength(50)]
        public string? import_permit_no { get; set; }
        [StringLength(1024)]
        public string? internal_remarks { get; set; }
        public int? forwarder_agent_id { get; set; }
        [StringLength(50)]
        public string? forwarder_invoice_no { get; set; }
        [DecimalPrecision(8, 2)]
        public decimal? forwarder_invoice_amt { get; set; }
        public int? shipping_agent_id { get; set; }
        [StringLength(50)]
        public string? shipping_invoice_no { get; set; }
        [DecimalPrecision(8, 2)]
        public decimal? shipping_invoice_amt { get; set; }
        public int? insurance_agent_id { get; set; }
        public int? forwarder_invoice_currency_id { get; set; }
        public int? shipping_invoice_currency_id { get; set; }
        public List<BlAwbRequestDto>? bl_awb { get; set; }
        public HashSet<int> asnList { get; set; }
        public List<IshUpload>? Attachments { get; set; }
    }

    public class IshUpload
    {
        [StringLength(100)]
        public string upload_source { get; set; }
        [MaxFileCount(10)]
        public List<IFormFile> files { get; set; }
        [Required] 
        public string document_type { get; set; }
    }
}
