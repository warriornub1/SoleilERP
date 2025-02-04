using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.InboundShipments
{
    public class InboundShipment: BaseModel
    {
        /// <summary>
        /// PO No	ASNYYMM99999
        /// </summary>
        [StringLength(20)]
        public string inbound_shipment_no { get; set; }
       /// <summary>
       /// 0: New
       /// 1: Received
       /// 3: Closed
       /// 9: Cancelled
       /// </summary>
        [StringLength(2)]
        public string status_flag { get; set; }
        /// <summary>
        /// Freight Method	
        /// </summary>
        [StringLength(50)]
        public string freight_method { get; set; }
        /// <summary>
        /// Inco Term	
        /// </summary>
        [StringLength(50)]
        public string incoterm { get; set; }
        /// <summary>
        /// Port Of Loading Est To Departure	
        /// </summary>
        public DateOnly? port_of_loading_etd { get; set; }
        /// <summary>
        /// Port Of Discharge Est To Arrival	
        /// </summary>
        public DateOnly? port_of_discharge_eta { get; set; }
        /// <summary>
        /// Country Of Loading	FK from table Country
        /// </summary>
        public int country_of_loading_id { get; set; }
        /// <summary>
        /// Port Of Loading ID	FK from table Port
        /// </summary>
        public int port_of_loading_id { get; set; }
        /// <summary>
        /// Country Of Discharge ID	FK from table Country
        /// </summary>
        public int country_of_discharge_id { get; set; }
        /// <summary>
        /// Port Of Discharge ID	FK from table Port
        /// </summary>
        public int port_of_discharge_id { get; set; }
        /// <summary>
        /// Vessel Flight No	
        /// </summary>
        [StringLength(50)]
        public string? vessel_flight_no { get; set; }
        /// <summary>
        /// Connecting Vessel Flight No	
        /// </summary>
        [StringLength(50)]
        public string? connecting_vessel_flight_no { get; set; }
        /// <summary>
        /// Notice Of Arrival Date
        /// </summary>
        public DateOnly? notice_of_arrival_date { get; set; }
        /// <summary>
        /// Import Permit No
        /// </summary>
        [StringLength(50)]
        public string? import_permit_no { get; set; }
        /// <summary>
        /// Internal Remarks
        /// </summary>
        [StringLength(1024)]
        public string? internal_remarks { get; set; }
        /// <summary>
        /// Forwarder Agent ID	FK from table Agent		NO	YES
        /// </summary>
        public int? forwarder_agent_id { get; set; }
        /// <summary>
        /// forwarder_invoice_no	VARCHAR	50	Forwarder Invoice No
        /// </summary>
        [StringLength(50)]
        public string? forwarder_invoice_no { get; set; }
        /// <summary>
        /// forwarder_invoice_currency_id	FK from table Currency		NO	YES
        /// </summary>
        public int?  forwarder_invoice_currency_id { get; set; }
        /// <summary>
        /// forwarder_invoice_amt	DECIMAL	8,2	Forwarder Invoice Amount
        /// </summary>
        [Column(TypeName = "decimal(8,2)")]
        public decimal? forwarder_invoice_amt { get; set; }
        /// <summary>
        /// Shipping Agent ID	FK from table Agent		NO	YES
        /// </summary>
        public int? shipping_agent_id { get; set; }
        /// <summary>
        /// Shipping Invoice No
        /// </summary>
        [StringLength(50)]
        public string? shipping_invoice_no { get; set; }
        /// <summary>
        /// Shipping Invoice Currency ID	FK from table Currency		NO	YES
        /// </summary>
        public int? shipping_invoice_currency_id { get; set; }
        /// <summary>
        /// Shipping Invoice Amount
        /// </summary>
        [Column(TypeName = "decimal(8,2)")]
        public decimal? shipping_invoice_amt { get; set; }
        /// <summary>
        /// Insurance Agent ID	FK from table Agent		NO	YES
        /// </summary>
        public int? insurance_agent_id { get; set; }
    }
}
