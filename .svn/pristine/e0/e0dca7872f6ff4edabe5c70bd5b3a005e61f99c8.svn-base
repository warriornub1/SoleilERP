namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class UpdatePackingForAsnRequestDto
    {
        public int container_id { get; set; }
        public int packing_header_id { get; set; }
        public List<UpdatePackingInformationRequestDto>? items { get; set; }
        public List<int>? delete_id { get; set; }
    }

    public class UpdatePackingInformationRequestDto: PackingInformationRequestDto
    {
        public int id { get; set; }
    }
}
