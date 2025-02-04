namespace SERP.Application.Masters.Suppliers.DTOs.Response
{
    public class ImportSupplierSecondaryData
    {
        /// <summary>
        /// Supplier No.
        /// </summary>
        public string? supplier_no { get; set; }
        /// <summary>
        /// Secondary Supplier No.
        /// </summary>
        public string? sec_supplier_no { get; set; }
        /// <summary>
        /// Secondary Supplier Name
        /// </summary>
        public string? sec_supplier_name { get; set; }

        public int index { get; set; }
    }
}
