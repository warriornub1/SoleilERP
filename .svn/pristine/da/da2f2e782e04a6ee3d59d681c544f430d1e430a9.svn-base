using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.Groups.Interfaces;
using SERP.Application.Finance.Natural_Accounts.DTOs.Request;
using SERP.Application.Masters.Lovs.Interfaces;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Validator
{
    public class INaturalAccountDtoValidator : AbstractValidator<NaturalAccountBaseDto>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ILovRepository _lovRepository;
        public INaturalAccountDtoValidator(IGroupRepository groupRepository, ILovRepository lovRepository)
        {
            _groupRepository = groupRepository;
            _lovRepository = lovRepository;


            RuleFor(x => x.natural_account_type).NotEmpty().NotNull()
                .MustAsync(async (natural_account_type, token) =>
                {
                    var parentGroupExists = await _lovRepository.GetByIdAsync(x => x.lov_value == natural_account_type && x.lov_type == ApplicationConstant.LovType.NaturalAccountTypes.ToString());
                    return parentGroupExists != null;
                })
                .WithMessage("{PropertyName}: {PropertyValue} not found.");



            RuleFor(x => x.parent_group_id)
                .GreaterThan(0)
                .MustAsync(async (parent_group_id, token) =>
                {
                    var parentGroupExists = await _groupRepository.GetByIdAsync(x => x.id == parent_group_id && x.group_type == DomainConstant.Group.GroupType.NaturalAccount && x.status_flag == DomainConstant.StatusFlag.Enabled);
                    return parentGroupExists != null;
                })
                .WithMessage(string.Format(ErrorMessages.ParentGroupIDDontExist, "{PropertyValue}"));

            RuleFor(x => x.status_flag).NotEmpty().NotNull();
 
        }
    }
}
