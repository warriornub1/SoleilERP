using SERP.Application.Masters.Suppliers.DTOs.Response;
using SERP.Domain.Masters.Suppliers;

namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class ValidateSupplierSecondaryRequest
    {
        public List<ImportSupplierSecondaryData> ExcelData { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
}
