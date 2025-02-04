using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SERP.Domain.Finance.CompanyStructures;
using SERP.Domain.Finance.Employees;
using SERP.Infrastructure.Finance.EmployeeStructureMappings;

namespace SERP.Infrastructure.Common.DBContexts.Config
{
    public class EmployeeStructureMappingConfig : IEntityTypeConfiguration<EmployeeStructureMapping>
    {
        public void Configure(EntityTypeBuilder<EmployeeStructureMapping> builder)
        {
            builder.HasIndex(g => new { g.employee_id, g.company_structure_id })
                   .IsUnique();

            builder.HasOne<CompanyStructure>()
                   .WithMany()
                   .HasForeignKey(g => g.company_structure_id)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Employee>()
                   .WithMany()
                   .HasForeignKey(g => g.employee_id)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
