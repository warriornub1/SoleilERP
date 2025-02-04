using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.BranchPlants.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Masters.Agents;
using SERP.Domain.Masters.Agents.Model;
using SERP.Domain.Masters.BranchPlants;
using SERP.Domain.Masters.BranchPlants.Models;
using SERP.Domain.Masters.Companies;
using SERP.Domain.Masters.Companies.Models;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Masters.Sites;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.BranchPlants
{
    public class BranchPlantRepository : GenericRepository<BranchPlant>, IBranchPlantRepository
    {
        public BranchPlantRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<List<BranchPlant>> GetListBranchPlantAsync(List<int> branchPlantIds)
        {
            var branchPlant = await _dbContext.BranchPlant.Where(x => branchPlantIds.Contains(x.id)).ToListAsync();
            return branchPlant;
        }
        public async Task<IEnumerable<BranchPlantDetail>> GetByCompany(string companyNo)
        {
            var result = await (from branchPlant in _dbContext.BranchPlant
                                join company in _dbContext.Company on branchPlant.company_id equals company.id
                                where company.company_no == companyNo
                                && branchPlant.status_flag == ApplicationConstant.StatusFlag.Enabled
                                select new BranchPlantDetail
                                {
                                    id = branchPlant.id,
                                    branch_plant_no = branchPlant.branch_plant_no,
                                    branch_plant_name = branchPlant.branch_plant_name,
                                    company = (from branchPlant in _dbContext.BranchPlant
                                               join company in _dbContext.Company on branchPlant.company_id equals company.id
                                               where branchPlant.company_id == company.id
                                               select new CompanyDetail
                                               {
                                                   id = company.id,
                                                   company_no = company.company_no,
                                                   company_name = company.company_name
                                               }).FirstOrDefault(),
                                    site = (from site in _dbContext.Site
                                            join country in _dbContext.Country on site.country_id equals country.id
                                            where branchPlant.site_id == site.id
                                            select new BranchPlantDetailSite
                                            {
                                                site_id = site.id,
                                                site_no = site.site_no,
                                                site_name = site.site_name,
                                                address_line_1 = site.address_line_1,
                                                address_line_2 = site.address_line_2,
                                                address_line_3 = site.address_line_3,
                                                address_line_4 = site.address_line_4,
                                                postal_code = site.postal_code,
                                                state_province = site.state_province,
                                                county = site.county,
                                                city = site.city,
                                                country = new CountryDetail
                                                {
                                                    country_id = country.id,
                                                    country_name = country.country_name,
                                                    country_long_name = country.country_long_name,
                                                    country_alpha_code_two = country.country_alpha_code_two,
                                                    country_alpha_code_three = country.country_alpha_code_three,
                                                    country_idd = country.country_idd,
                                                    continent = country.continent
                                                }
                                            }).FirstOrDefault()
                                }).ToListAsync();

            return result;
        }

        public async Task<int[]> CheckInvalidBranchPlantIds(HashSet<int> branchPlantIds)
        {
            var query = await (from branchPlant in _dbContext.BranchPlant
                               where branchPlantIds.Contains(branchPlant.id) && branchPlant.status_flag.Equals(DomainConstant.StatusFlag.Enabled)
                               select branchPlant.id).ToArrayAsync();
            var result = branchPlantIds.Except(query);
            return result.ToArray();
        }

        public async Task<int[]> GetBranchPlantAvailable(HashSet<int> branchPlantIds)
        {
            var branchPlantIDs = await _dbContext.BranchPlant.Where(x =>
                    x.status_flag == ApplicationConstant.StatusFlag.Enabled && branchPlantIds.Contains(x.id))
                 .Select(x => x.id).ToArrayAsync();

            return branchPlantIDs;
        }

        public async Task<bool> CheckExistedBranchPlantNo(string branchPlantNo)
        {
            return await _dbContext.BranchPlant.AnyAsync(x => x.branch_plant_no.Equals(branchPlantNo));
        }

        public async Task<BranchPlantDetail> GetById(int id)
        {
            var result = await (from branchPlant in _dbContext.BranchPlant
                                where branchPlant.id == id
                                select new BranchPlantDetail
                                {
                                    id = branchPlant.id,
                                    branch_plant_no = branchPlant.branch_plant_no,
                                    branch_plant_name = branchPlant.branch_plant_name,
                                    company = (from BP in _dbContext.BranchPlant
                                               join company in _dbContext.Company on BP.company_id equals company.id
                                               where BP.company_id == company.id
                                               select new CompanyDetail
                                               {
                                                   id = company.id,
                                                   company_no = company.company_no,
                                                   company_name = company.company_name
                                               }).FirstOrDefault(),
                                    status_flag = branchPlant.status_flag,
                                    site = (from site in _dbContext.Site
                                            join country in _dbContext.Country on site.country_id equals country.id
                                            where branchPlant.site_id == site.id
                                            select new BranchPlantDetailSite
                                            {
                                                site_id = site.id,
                                                site_no = site.site_no,
                                                site_name = site.site_name,
                                                address_line_1 = site.address_line_1,
                                                address_line_2 = site.address_line_2,
                                                address_line_3 = site.address_line_3,
                                                address_line_4 = site.address_line_4,
                                                postal_code = site.postal_code,
                                                state_province = site.state_province,
                                                county = site.county,
                                                city = site.city,
                                                country = new CountryDetail
                                                {
                                                    country_id = country.id,
                                                    country_name = country.country_name,
                                                    country_long_name = country.country_long_name,
                                                    country_alpha_code_two = country.country_alpha_code_two,
                                                    country_alpha_code_three = country.country_alpha_code_three,
                                                    country_idd = country.country_idd,
                                                    continent = country.continent
                                                }
                                            }).FirstOrDefault()
                                }).FirstAsync();

            return result;
        }

        public async Task<IEnumerable<int>> FindMapping(List<int> ids)
        {
            return await _dbContext.BranchPlant.Where(x => ids.Contains(x.company_id))
                                                   .Select(x => x.company_id)
                                                   .ToListAsync();

        }
        public async Task<int?> GetCompanyIdsByBranchPlantId(int branchPlantId)
        {
            return await _dbContext.BranchPlant.AsNoTracking()
                .Where(x => x.id == branchPlantId)
                .Select(x => x.company_id).FirstOrDefaultAsync();
        }

        public IQueryable<PagedBranchPlantDetail> BuildFilterBranchPlantQuery(PagedFilterBranchPlantRequestModel request)
        {
            var branchPlantQuery = _dbContext.BranchPlant.AsNoTracking();

            // create_date_from
            if (request.create_date_from.HasValue)
            {
                branchPlantQuery = branchPlantQuery.Where(x => x.created_on >= request.create_date_from);
            }

            // create_date_to
            if (request.create_date_to.HasValue)
            {
                branchPlantQuery = branchPlantQuery.Where(x => x.created_on <= request.create_date_to);
            }

            // company_id
            //if (request.company_id is not null && request.company_id.Count > 0)
            //{
            //    branchPlantQuery = branchPlantQuery.Where(x => request.company_id.Contains(x.company_id));
            //}

            //// status_flag
            //if (request.status_flag is not null && request.status_flag.Count > 0)
            //{
            //    branchPlantQuery = branchPlantQuery.Where(x => request.status_flag.Contains(x.status_flag));
            //}

            var branchPlantDetail = from branchPlant in branchPlantQuery
                                    select new PagedBranchPlantDetail
                                    {
                                        id = branchPlant.id,
                                        branch_plant_no = branchPlant.branch_plant_no,
                                        branch_plant_name = branchPlant.branch_plant_name,
                                        company = (from bp in branchPlantQuery
                                                   join company in _dbContext.Company on bp.company_id equals company.id
                                                   where bp.company_id == company.id
                                                   select new CompanyDetail
                                                   {
                                                       id = company.id,
                                                       company_no = company.company_no,
                                                       company_name = company.company_name
                                                   }).FirstOrDefault(),
                                        site = (from site in _dbContext.Site
                                                join country in _dbContext.Country on site.country_id equals country.id
                                                where branchPlant.site_id == site.id
                                                select new PagedBranchPlantSiteDetail
                                                {
                                                    site_id = site.id,
                                                    site_no = site.site_no,
                                                    site_name = site.site_name,
                                                    address_line_1 = site.address_line_1,
                                                    address_line_2 = site.address_line_2,
                                                    address_line_3 = site.address_line_3,
                                                    address_line_4 = site.address_line_4,
                                                    postal_code = site.postal_code,
                                                    state_province = site.state_province,
                                                    county = site.county,
                                                    city = site.city,
                                                    country = new CountryDetail
                                                    {
                                                        country_id = country.id,
                                                        country_name = country.country_name,
                                                        country_long_name = country.country_long_name,
                                                        country_alpha_code_two = country.country_alpha_code_two,
                                                        country_alpha_code_three = country.country_alpha_code_three,
                                                        country_idd = country.country_idd,
                                                        continent = country.continent
                                                    }
                                                }).FirstOrDefault()
                                    };


            if (!string.IsNullOrEmpty(request.Keyword))
            {
                branchPlantDetail = branchPlantDetail.Where(x =>
                x.branch_plant_name.Contains(request.Keyword) ||
                    x.branch_plant_no.Contains(request.Keyword));
            }

            return branchPlantDetail;
        }
        public async Task<IEnumerable<int>> GetBranchPlantId(List<int> ids)
        {
            return await _dbContext.BranchPlant.Where(x => ids.Contains(x.company_id))
                                               .Select(x => x.company_id)
                                               .ToListAsync();
        }
    }
}