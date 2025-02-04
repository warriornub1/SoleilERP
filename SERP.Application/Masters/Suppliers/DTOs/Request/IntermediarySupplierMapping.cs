using SERP.Domain.Masters.Suppliers;

namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class IntermediarySupplierMapping
    {
        public Supplier Supplier { get; set; }
        public string IntermediarySupplierNo { get; set; }
        public IntermediarySupplier IntermediarySupplier { get; set; }
    }
}
