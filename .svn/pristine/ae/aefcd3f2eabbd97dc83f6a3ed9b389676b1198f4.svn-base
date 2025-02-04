using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SERP.Domain.Common.Constants;
using SERP.Domain.Finance.Employees;

namespace SERP.Infrastructure.Common.DBContexts.Config
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            var validStatusTypes = new[]
{
                DomainConstant.StatusFlag.Enabled,
                DomainConstant.StatusFlag.Disabled
            };

            builder.HasCheckConstraint("CK_Employee_StatusFlag",
                $"status_flag IN ({string.Join(", ", validStatusTypes.Select(t => $"'{t}'"))})");

        }
    }
}
