using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Customers.Interfaces;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Masters.Customers;
using SERP.Domain.Masters.Customers.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Customers
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Customer>> GetAllLimited(bool onlyEnabled)
        {
            return await _dbContext.Customer.Where(x => !onlyEnabled || x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled)).ToListAsync();
        }

        public async Task<IEnumerable<CustomerShipToDetail>> GetAllShipToByCustomer(int customerId, bool onlyEnabled)
        {
            var result = await (from customerShipTo in _dbContext.CustomerShipTo
                                where customerShipTo.customer_id == customerId
                                && (!onlyEnabled || customerShipTo.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled))
                                select new CustomerShipToDetail
                                {
                                    customer_ship_to_id = customerShipTo.id,
                                    default_flag = customerShipTo.default_flag,
                                    status_flag = customerShipTo.status_flag,
                                    ship_to_site = (from shipToSite in _dbContext.Site
                                                    join country in _dbContext.Country on shipToSite.country_id equals country.id
                                                    where customerShipTo.site_id == shipToSite.id
                                                    select new CustomerShipToDetailSite
                                                    {
                                                        site_id = shipToSite.id,
                                                        site_no = shipToSite.site_no,
                                                        site_name = shipToSite.site_name,
                                                        address_line_1 = shipToSite.address_line_1,
                                                        address_line_2 = shipToSite.address_line_2,
                                                        address_line_3 = shipToSite.address_line_3,
                                                        address_line_4 = shipToSite.address_line_4,
                                                        postal_code = shipToSite.postal_code,
                                                        state_province = shipToSite.state_province,
                                                        county = shipToSite.county,
                                                        city = shipToSite.city,
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

    }
}
