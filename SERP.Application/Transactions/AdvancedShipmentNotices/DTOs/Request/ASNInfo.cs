using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.Containers;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.Invoice;
using SERP.Domain.Transactions.PackingLists;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class ASNInfo
    {
        public InboundShipment? InboundShipment { get; set; }
        public InboundShipmentBLAWB? InboundShipmentBlAwb { get; set; }
        public InboundShipmentRequest? InboundShipmentRequest { get; set; }
        public ASNHeader Header { get; set; }
        public List<ASNDetail>? Details { get; set; }
        public List<Container>? Containers { get; set; }
        public List<Container>? ExistingContainers { get; set; }
        public List<InvoiceHeader>? InvoiceHeaders { get; set; }
        public List<InvoiceMapping>? InvoiceMapping { get; set; }
        public List<ASNPackingDiscrepancy>? ASNPackingLists { get; set; }
        //public List <ContainerPackingListMapping>? PackingListMappings { get; set; }
    }

    //public class ContainerPackingListMapping
    //{
    //    public string container_no { get; set; }
    //    public List <PackingList> PackingLists { get; set; }
    //}

    public class InvoiceMapping
    {
        public InvoiceHeader InvoiceHeader { get; set; }
        public List<InvoiceDetail>? InvoiceDetails { get; set; }
    }
}
