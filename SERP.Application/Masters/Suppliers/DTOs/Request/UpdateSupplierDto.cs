using FluentValidation;
using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class UpdateSupplierDto
    {
        public int id { get; set; }
        [StringLength(50)]
        public string supplier_no { get; set; }
        [StringLength(100)]
        public string supplier_name { get; set; }
        public int? registered_site_id { get; set; }
        [AcceptValue(StatusFlag.Enabled, StatusFlag.Disabled)]
        public string status_flag { get; set; }
        public bool service_flag { get; set; }
        public bool product_flag { get; set; }
        public bool authorised_flag { get; set; }
        [StringLength(50)]
        public string? payment_term { get; set; }
        [StringLength(50)]
        public string? payment_method { get; set; }
        public int default_currency_id { get; set; }
        [StringLength(50)]
        public string? landed_cost_rule { get; set; }
        [StringLength(50)]
        public string? incoterm { get; set; }
        [StringLength(50)]
        public string? default_freight_method { get; set; }
        [StringLength(50)]
        public string? po_sending_method { get; set; }
        public int? default_country_of_loading_id { get; set; }
        public int? default_port_of_loading_id { get; set; }
        public int? default_country_of_discharge_id { get; set; }
        public int? default_port_of_discharge_id { get; set; }
    }
    public class UpdateSupplierDtoValidator : AbstractValidator<UpdateSupplierDto>
    {
        public UpdateSupplierDtoValidator()
        {
            RuleFor(x => x.id).NotEmpty().NotNull();
            RuleFor(x => x.supplier_no).NotEmpty().NotNull();
            RuleFor(x => x.status_flag).NotEmpty().NotNull();
            RuleFor(x => x.supplier_name).NotEmpty().NotNull();
            RuleFor(x => x.service_flag).NotEmpty().NotNull();
            RuleFor(x => x.product_flag).NotEmpty().NotNull();
            RuleFor(x => x.authorised_flag).NotEmpty().NotNull();
        }

    }
}
