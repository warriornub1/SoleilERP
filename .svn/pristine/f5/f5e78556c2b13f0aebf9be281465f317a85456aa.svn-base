using FluentValidation;
using SERP.Application.Transactions.FilesTracking.DTOs;

namespace SERP.Application.Transactions.Containers.DTOs.Request
{
    public class ContainerFileInfoDto: FileUploadBaseRequestDto
    {
        public string container_file_type { get; set; }

        public class ContainerFileInfoDtoValidator : AbstractValidator<ContainerFileInfoDto>
        {
            public ContainerFileInfoDtoValidator()
            {
                RuleFor(x => x.file_type).NotEmpty().NotNull();
                RuleFor(x => x.document_type).NotEmpty().NotNull();
                RuleFor(x => x.container_file_type).NotEmpty().NotNull();
                RuleFor(x => x.upload_source).NotEmpty().NotNull();
                RuleFor(x => x.file).NotEmpty().NotNull();
            }
        }
        public class ContainerFileInfoDtoListValidator : AbstractValidator<List<ContainerFileInfoDto>>
        {
            public ContainerFileInfoDtoListValidator()
            {
                // Use RuleForEach to validate each item in the Items list
                RuleForEach(x => x).SetValidator(new ContainerFileInfoDtoValidator());
            }
        }
    }
}
