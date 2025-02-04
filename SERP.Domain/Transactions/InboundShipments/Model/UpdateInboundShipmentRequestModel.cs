using SERP.Domain.Transactions.InboundShipments.Model.Base;

namespace SERP.Domain.Transactions.InboundShipments.Model
{
    public class UpdateInboundShipmentRequestModel: CreateInboundShipmentRequestModel
    {
        public int id { get; set; }
        [Obsolete]
        public new List<UpdateBlAwbInfoRequestModel>? bl_awb { get; set; }
        [Obsolete]
        public new HashSet<int>? asnList { get; set; }

    }

    public class UpdateBlAwbInfoRequestModel: BlAwbRequestModel
    {
        public int bl_awb_id { get; set; }
    }
}
