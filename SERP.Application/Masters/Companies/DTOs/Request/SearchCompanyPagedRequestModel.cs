using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Masters.Companies.DTOs.Request
{
    public class SearchCompanyPagedRequestModel
    {
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public List<ParentGroupListCO>? parentGroupList { get; set; }
        public List<CurrencyListCO>? currencyList { get; set; }
        public List<IntercompanyFlagListCO>? intercompanyFlagList { get; set; }
        public List<DormantFlagListCO>? dormantFlagList { get; set; }
        public List<StatusListCO>? statusList { get; set; }

        public class SearchCompanyPagedRequestModelValidator : AbstractValidator<SearchCompanyPagedRequestModel>
        {
            public SearchCompanyPagedRequestModelValidator()
            {

                When(x => x.create_date_to != null && x.create_date_from != null, () =>
                {
                    RuleFor(x => x.create_date_from)
                        .NotNull().WithMessage("EndDate is required.")
                        .GreaterThan(x => x.create_date_from).WithMessage("EndDate must be after StartDate.");


                });

                // Define rules for parentGroupList
                When(x => x.parentGroupList != null && x.parentGroupList.Any(), () =>
                {
                    RuleForEach(x => x.parentGroupList)
                        .Must(x => x.group_id > 0)
                        .WithMessage("Invalid group_id. group_id must be greater than 0.");
                });

                // Define rules for currencyList
                When(x => x.currencyList != null && x.currencyList.Any(), () =>
                {
                    RuleForEach(x => x.currencyList)
                        .Must(x => x.currency_id > 0)
                        .WithMessage("Invalid currency_id. currency_id must be greater than 0.");
                });

                // Define rules for statusList
                When(x => x.statusList != null && x.statusList.Any(), () =>
                {
                    RuleForEach(x => x.statusList)
                        .NotEmpty()
                        .WithMessage("Status cannot be empty.");
                });

                When(x => x.statusList != null && x.statusList.Any(), () =>
                {
                    RuleForEach(x => x.statusList)
                        .Must(x =>
                            x.status == DomainConstant.StatusFlag.Enabled ||
                            x.status == DomainConstant.StatusFlag.Disabled)
                        .WithMessage(string.Format(ErrorMessages.CompanyFlagNotAccepted, "status_flag"));
                });

            }

        }
    }

    public class ParentGroupListCO
    {
        public int group_id { get; set; }
    }

    public class CurrencyListCO
    {
        public int currency_id { get; set; }
    }
    public class IntercompanyFlagListCO
    {
        public bool flag { get; set; }
    }

    public class DormantFlagListCO
    {
        public bool flag { get; set; }
    }

    public class StatusListCO
    {
        public string status { get; set; }
    }
}
