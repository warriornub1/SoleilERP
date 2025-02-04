using SERP.Application.Finance.EmployeeStructureMappings.Interface;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Finance.EmployeeStructureMappings
{
    public class EmployeeStructureMappingRepository : GenericRepository<EmployeeStructureMapping>, IEmployeeStructureMappingRepository
    {
        public EmployeeStructureMappingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
