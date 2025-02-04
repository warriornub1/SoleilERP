using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SERP.Application.Finance.CompanyStructures.DTOs.Response;
using SERP.Application.Finance.CompanyStructures.Interface;
using SERP.Domain.Common.Constants;
using SERP.Domain.Finance.CompanyStructures;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Finance.CompanyStructures
{
    public class CompanyStructureRepository : GenericRepository<CompanyStructure>, ICompanyStructureRepository
    {
        public CompanyStructureRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<int>> GetCompanyStructureCodes(List<int> companyStructureId)
        {
            return await _dbContext.CompanyStructures.Where(x => companyStructureId.Contains(x.id))
                                                     .Select(x => x.id)
                                                     .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetCompanyOrgNoDic(List<string> orgNumber)
        {
            return await _dbContext.CompanyStructures.Where(x => orgNumber.Contains(x.org_no))
                                                     .Select(x => new { x.org_no, x.id })
                                                     .ToDictionaryAsync(x => x.org_no, x => x.id);
        }

        public async Task<Dictionary<int, string>> GetCompanyOrgIdDic(List<int> idNos)
        {
            return await _dbContext.CompanyStructures.Where(x => idNos.Contains(x.id))
                                                     .Select(x => new { x.org_no, x.id })
                                                     .ToDictionaryAsync(x => x.id, x => x.org_no);
        }

        public async Task<IEnumerable<SearchCompanyStructureResponseModel>> GetCompanyStructSearchPage(string keyword, string sortBy, bool sortAscending,
                                                        List<int> companyList, List<int> orgList, List<string> statusList,
                                                        List<int> inChargeEmployeeIdList, List<int> departmentList, List<int> divisonList, List<int> sectionList)
        {
            var queryCS = _dbContext.CompanyStructures.AsQueryable();
            var queryES = _dbContext.EmployeeStructureMappings.AsQueryable();
            var queryE = _dbContext.Employees.AsQueryable();

            if(!string.IsNullOrEmpty(keyword))
            {
                queryCS = queryCS.Where(x => x.org_description.Contains(keyword) || x.org_code.Contains(keyword) || x.org_no.Contains(keyword) );
            }

            if(companyList is not null && companyList.Count > 0)
            {
                queryCS = queryCS.Where(x => companyList.Contains(x.company_id));
            }

            if (orgList is not null && orgList.Count > 0)
            {
                queryCS = queryCS.Where(x => orgList.Contains(x.org_type));
            }

            if (statusList is not null && statusList.Count > 0)
            {
                queryCS = queryCS.Where(x => statusList.Contains(x.status_flag));
            }

            if (inChargeEmployeeIdList is not null && inChargeEmployeeIdList.Count > 0)
            {
                queryCS = queryCS.Where(x => inChargeEmployeeIdList.Contains(x.in_charge_employee_id.Value));
            }

            if (departmentList is not null && departmentList.Count > 0)
            {
                queryCS = queryCS.Where(x => departmentList.Contains(x.id));
            }

            if (divisonList is not null && divisonList.Count > 0)
            {
                queryCS = queryCS.Where(x => divisonList.Contains(x.id));
            }

            if (sectionList is not null && sectionList.Count > 0)
            {
                queryCS = queryCS.Where(x => sectionList.Contains(x.id));
            }

            var query = from cs in queryCS
                        join es in queryES on cs.id equals es.company_structure_id
                        join e in queryE on es.employee_id equals e.id
                        select new SearchCompanyStructureResponseModel
                        {
                            id = cs.id,
                            company_id = cs.company_id,
                            sequence = cs.sequence,
                            org_no = cs.org_no,
                            org_code = cs.org_code,
                            org_description = cs.org_description,
                            status_flag = cs.status_flag,
                            org_type = cs.org_type,
                            top_flag = cs.top_flag,
                            in_charge_employee = new InChargeEmployee
                            {
                                id = e.id,
                                employee_no = e.employee_no,
                                employee_name = e.employee_name,
                                alias = e.ailas,
                            },
                            parent_id = cs.parent_id,
                            created_on = cs.created_on,
                            created_by = cs.created_by,
                            last_modified_on = cs.last_modified_on,
                            last_modified_by = cs.last_modified_by,
                        };

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortAscending
                    ? query.OrderBy(x => EF.Property<object>(x, sortBy))
                    : query.OrderByDescending(x => EF.Property<object>(x, sortBy));
            }

            return await query.ToListAsync();

        }

        public async Task<IEnumerable<CompanyStructure>> GeCompanyStructureLimited(int company_id, int? org_type, bool onlyEnabled)
        {
            var queryCS = _dbContext.CompanyStructures.AsQueryable();
            if(org_type != 0)
            {
                queryCS = queryCS.Where(x => x.org_type == org_type);
            }
            
            if(onlyEnabled)
            {
                queryCS = queryCS.Where(x => x.status_flag == DomainConstant.StatusFlag.Enabled);
            }

            return await queryCS.Where(x => x.company_id == company_id)
                                .ToListAsync();
        }

    }
}
