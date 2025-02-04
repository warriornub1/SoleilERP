using FluentValidation;

namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class CreateSupplierDto
    {
        public int registered_site_id { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public string status_flag { get; set; }
        public bool service_flag { get; set; }
        public bool product_flag { get; set; }
        public bool authorised_flag { get; set; }
        public string? payment_term { get; set; }
        public string? payment_method { get; set; }
        public int default_currency_id { get; set; }
        public string? landed_cost_rule { get; set; }
        public string? incoterm { get; set; }
        public string? default_freight_method { get; set; }
        public string? po_sending_method { get; set; }
        public int? default_country_of_loading_id { get; set; }
        public int? default_port_of_loading_id { get; set; }
        public int? default_country_of_discharge_id { get; set; }
        public int? default_port_of_discharge_id { get; set; }
    }

    public class CreateSupplierDtoValidator : AbstractValidator<CreateSupplierDto>
    {
        public CreateSupplierDtoValidator()
        {
            RuleFor(x => x.supplier_no).NotEmpty().NotNull();
            RuleFor(x => x.status_flag).NotEmpty().NotNull();
            RuleFor(x => x.supplier_name).NotEmpty().NotNull();
            RuleFor(x => x.service_flag).NotEmpty().NotNull();
            RuleFor(x => x.product_flag).NotEmpty().NotNull();
            RuleFor(x => x.authorised_flag).NotEmpty().NotNull();
        }

    }
}
