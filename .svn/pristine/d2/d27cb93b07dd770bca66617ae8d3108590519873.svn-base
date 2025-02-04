using SERP.Domain.Transactions.Containers;
using SERP.Domain.Transactions.FilesTracking;
using SERP.Domain.Transactions.InboundShipments;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Request
{
    public class InboundShipmentInfoMapping
    {
        public required InboundShipment InboundShipment { get; set; }
        public List<BlAwbContainerMapping> BlAwb { get; set; }
        public HashSet<int>? AsnList { get; set; }
        public List<FileTracking> FileTrackings { get; set; }
    }

    public class BlAwbContainerMapping
    {
        public InboundShipmentBLAWB InboundShipmentBlAwb { get; set; }
        public List<Container> Containers { get; set; }
    }
}
