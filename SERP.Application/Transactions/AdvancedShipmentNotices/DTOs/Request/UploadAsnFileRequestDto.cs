using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request;

public class UploadAsnFileRequestDto
{
    public int asn_header_id { get; set; }
    public string upload_source { get; set; }
    public List<FileInfoRequestDto> files { get; set; }

}