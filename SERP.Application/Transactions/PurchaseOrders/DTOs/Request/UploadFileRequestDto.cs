namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request;

public class UploadFileRequestDto
{
    public int po_header_id { get; set; }
    public string upload_source { get; set; }
    public List<FileInfoRequestDto> files { get; set; }

}