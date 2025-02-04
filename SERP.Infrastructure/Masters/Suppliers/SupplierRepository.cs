using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Masters.Items.Model;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SERP.Infrastructure.Masters.Suppliers
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Supplier>> GetAllLimited(bool onlyEnabled)
        {
            return await _dbContext.Supplier
                .Where(x => !onlyEnabled || x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled))
                .OrderBy(x => x.supplier_no).ToListAsync();
        }

        public async Task<SupplierDetail?> GetById(int supplierId)
        {
            var result = await (from supplier in _dbContext.Supplier
                                join defaultCurrency in _dbContext.Currency on supplier.default_currency_id equals defaultCurrency.id
                                join defaultCountryOfLoading in _dbContext.Country on supplier.default_country_of_loading_id equals
                                    defaultCountryOfLoading.id into countryOfLoadingGroup
                                from defaultCountryOfLoading in countryOfLoadingGroup.DefaultIfEmpty()
                                join defaultPortOfLoading in _dbContext.Ports on supplier.default_port_of_loading_id equals
                                    defaultPortOfLoading.id into portOfLoadingGroup
                                from defaultPortOfLoading in portOfLoadingGroup.DefaultIfEmpty()
                                join defaultCountryOfDischarge in _dbContext.Country on supplier.default_country_of_discharge_id equals
                                    defaultCountryOfDischarge.id into countryOfDischargeGroup
                                from defaultCountryOfDischarge in countryOfDischargeGroup.DefaultIfEmpty()
                                join defaultPortOfDischarge in _dbContext.Ports on supplier.default_port_of_discharge_id equals
                                    defaultPortOfDischarge.id into portOfDischargeGroup
                                from defaultPortOfDischarge in portOfDischargeGroup.DefaultIfEmpty()
                                where supplier.id == supplierId
                                select new SupplierDetail
                                {
                                    id = supplier.id,
                                    supplier_no = supplier.supplier_no,
                                    supplier_name = supplier.supplier_name,
                                    status_flag = supplier.status_flag,
                                    service_flag = supplier.service_flag,
                                    product_flag = supplier.product_flag,
                                    authorised_flag = supplier.authorised_flag,
                                    payment_term = supplier.payment_term,
                                    payment_method = supplier.payment_method,
                                    default_currency_id = supplier.default_currency_id,
                                    default_currency = defaultCurrency.currency_code,
                                    landed_cost_rule = supplier.landed_cost_rule,
                                    incoterm = supplier.incoterm,
                                    default_freight_method = supplier.default_freight_method,
                                    po_sending_method = supplier.po_sending_method,
                                    default_country_of_loading = defaultCountryOfLoading == null
                                        ? null
                                        : new CountryDetail
                                        {
                                            country_id = defaultCountryOfLoading.id,
                                            country_name = defaultCountryOfLoading.country_name,
                                            country_long_name = defaultCountryOfLoading.country_long_name,
                                            country_alpha_code_two = defaultCountryOfLoading.country_alpha_code_two,
                                            country_alpha_code_three = defaultCountryOfLoading.country_alpha_code_three,
                                            country_idd = defaultCountryOfLoading.country_idd,
                                            continent = defaultCountryOfLoading.continent
                                        },
                                    default_port_of_loading = defaultPortOfLoading == null
                                        ? null
                                        : new PortInfoDetail
                                        {
                                            port_id = defaultPortOfLoading.id,
                                            port_no = defaultPortOfLoading.port_no,
                                            port_name = defaultPortOfLoading.port_name
                                        },
                                    default_country_of_discharge = defaultCountryOfDischarge == null
                                        ? null
                                        : new CountryDetail
                                        {
                                            country_id = defaultCountryOfDischarge.id,
                                            country_name = defaultCountryOfDischarge.country_name,
                                            country_long_name = defaultCountryOfDischarge.country_long_name,
                                            country_alpha_code_two = defaultCountryOfDischarge.country_alpha_code_two,
                                            country_alpha_code_three = defaultCountryOfDischarge.country_alpha_code_three,
                                            country_idd = defaultCountryOfDischarge.country_idd,
                                            continent = defaultCountryOfDischarge.continent
                                        },
                                    default_port_of_discharge = defaultPortOfDischarge == null
                                        ? null
                                        : new PortInfoDetail
                                        {
                                            port_id = defaultPortOfDischarge.id,
                                            port_no = defaultPortOfDischarge.port_no,
                                            port_name = defaultPortOfDischarge.port_name
                                        },
                                    created_on = supplier.created_on,
                                    created_by = supplier.created_by,
                                    last_modified_by = supplier.last_modified_by,
                                    last_modified_on = supplier.last_modified_on,
                                    registered_site = new RegisteredSiteDetail()
                                    {
                                        site_id = supplier.registered_site_id,
                                    }
                                }).FirstOrDefaultAsync();


            if (result is null)
            {
                return null;
            }

            var intermediarySuppliers = (from intermediarySupplierMapping in _dbContext.IntermediarySupplier
                                         join intermediarySupplier in _dbContext.Supplier on intermediarySupplierMapping.int_supplier_id equals
                                             intermediarySupplier.id
                                         where intermediarySupplierMapping.supplier_id == supplierId
                                               && intermediarySupplier.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled)
                                               && intermediarySupplierMapping.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled)
                                         orderby intermediarySupplier.supplier_no
                                         select new IntermediarySupplierDetail
                                         {
                                             intermediary_supplier_id = intermediarySupplier.id,
                                             intermediary_supplier_no = intermediarySupplier.supplier_no,
                                             intermediary_supplier_name = intermediarySupplier.supplier_name,
                                             intermediary_supplier_status_flag = intermediarySupplier.status_flag,
                                             intermediary_supplier_default = intermediarySupplierMapping.default_flag,
                                             created_by = intermediarySupplierMapping.created_by,
                                             created_on = intermediarySupplierMapping.created_on,
                                         }).ToList();

            var secondarySuppliers = (from secondary_supplier in _dbContext.SecondarySupplier
                                      where secondary_supplier.supplier_id == supplierId
                                            && secondary_supplier.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled)
                                      orderby secondary_supplier.sec_supplier_no
                                      select new SecondarySupplierDetail
                                      {
                                          secondary_supplier_id = secondary_supplier.id,
                                          secondary_supplier_no = secondary_supplier.sec_supplier_no,
                                          secondary_supplier_name = secondary_supplier.sec_supplier_name,
                                          secondary_supplier_status_flag = secondary_supplier.status_flag,
                                          created_by = secondary_supplier.created_by,
                                          created_on = secondary_supplier.created_on,
                                          last_modified_by = secondary_supplier.last_modified_by,
                                          last_modified_on = secondary_supplier.last_modified_on
                                      }).ToList();

            var selfCollectSites = (from self_collect_site in _dbContext.SupplierSelfCollectSites
                                    join site in _dbContext.Site on self_collect_site.site_id equals site.id
                                    join country in _dbContext.Country on site.country_id equals country.id
                                    where self_collect_site.supplier_id == supplierId &&
                                          self_collect_site.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled)
                                    select new SelfCollectSiteDetail
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
                                        },
                                        created_by = self_collect_site.created_by,
                                        created_on = self_collect_site.created_on,
                                    }).ToList();

            var itemMappingInfo = await (from itemMapping in _dbContext.SupplierItemMapping.AsNoTracking()
                                         join item in _dbContext.Item.AsNoTracking() on itemMapping.item_id equals item.id
                                         where itemMapping.supplier_id == supplierId
                                         select new ItemMappingDetail
                                         {
                                             item_no = item.item_no,
                                             description_1 = item.description_1,
                                             description_2 = item.description_2,
                                             supplier_part_no = itemMapping.supplier_part_no,
                                             supplier_material_code = itemMapping.supplier_material_code,
                                             supplier_material_description = itemMapping.supplier_material_description,
                                             default_flag = itemMapping.default_flag,
                                             status_flag = itemMapping.status_flag,
                                             created_on = itemMapping.created_on,
                                             created_by = itemMapping.created_by,
                                             last_modified_on = itemMapping.last_modified_on,
                                             last_modified_by = itemMapping.last_modified_by
                                         }).ToListAsync();

            var registeredSite = (from site in _dbContext.Site
                join country in _dbContext.Country on site.country_id equals country.id into lstCountry
                from country in lstCountry.DefaultIfEmpty()
                where site.id == result.registered_site.site_id
                select new RegisteredSiteDetail()
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
                    country = country == null
                        ? null
                        : new CountryDetail
                        {
                            country_id = country.id,
                            country_name = country.country_name,
                            country_long_name = country.country_long_name,
                            country_alpha_code_two = country.country_alpha_code_two,
                            country_alpha_code_three = country.country_alpha_code_three,
                            country_idd = country.country_idd,
                            continent = country.continent
                        },
                }).FirstOrDefault();

            result.intermediary_suppliers = intermediarySuppliers;
            result.secondary_suppliers = secondarySuppliers;
            result.self_collect_sites = selfCollectSites;
            result.item_mapping = itemMappingInfo;
            result.registered_site = registeredSite;

            return result;
        }

        public async Task<IEnumerable<SecondarySupplier>> GetSecondarySupplierLimited(int supplierId, bool onlyEnabled)
        {
            return await _dbContext.SecondarySupplier
                .Where(x => x.supplier_id == supplierId && (!onlyEnabled || x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled)))
                .OrderBy(x => x.sec_supplier_no).ToListAsync();
        }

        public async Task<IEnumerable<SupplierItemMapping>> GetSupplierItemMapping(int supplierId, int itemId, bool onlyEnabled)
        {
            return await _dbContext.SupplierItemMapping.Where(x => x.supplier_id == supplierId && x.item_id == itemId && (!onlyEnabled || x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled))).ToListAsync();
        }

        public async Task<int[]> CheckInvalidSupplierIds(List<int> supplierIds)
        {
            var query = await _dbContext.Supplier
                .Where(x => supplierIds.Contains(x.id) && x.status_flag.Equals(DomainConstant.StatusFlag.Enabled))
                .Select(x => x.id).ToArrayAsync();

            var result = supplierIds.Except(query).ToArray();
            return result;
        }

        public async Task<List<IntermediarySupplier>> GetIntermediarySupplierList(HashSet<int> supplierId)
        {
            return await _dbContext.IntermediarySupplier
                .Where(x => supplierId.Contains(x.supplier_id))
                .ToListAsync();
        }

        public async Task<List<Supplier>> GetSupplierNo(HashSet<int> supplierIDs)
        {
            var supplier = await _dbContext.Supplier
                .Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled
                            && supplierIDs.Contains(x.id))
                .Select(x => new Supplier()
                {
                    id = x.id,
                    supplier_no = x.supplier_no
                }).ToListAsync();

            return supplier;
        }

        public async Task<int[]> GetSupplierAvailable(HashSet<int> supplierListIds)
        {
            return await _dbContext.Supplier
                .Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled &&
                            supplierListIds.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
        }

        public IQueryable<PagedSupplierDetail> BuildSupplierFilterQuery(PagedFilterSupplierRequestModel request, out int totalRows)
        {
            var supplierQuery = _dbContext.Supplier.AsNoTracking();

            if (request.product_flag.HasValue)
            {
                supplierQuery = supplierQuery.Where(x => x.product_flag == request.product_flag.Value);
            }

            if (request.service_flag.HasValue)
            {
                supplierQuery = supplierQuery.Where(x => x.service_flag == request.service_flag.Value);
            }

            var supplierMainQuery = from supplier in supplierQuery
                                    join registerSite in _dbContext.Site on supplier.registered_site_id equals registerSite.id
                                    select new
                                    {
                                        supplier,
                                        register_site_no = registerSite.site_no,
                                        register_site_name = registerSite.site_name
                                    };

            if (request.create_date_from.HasValue)
            {
                supplierMainQuery = supplierMainQuery.Where(x => x.supplier.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                supplierMainQuery = supplierMainQuery.Where(x => x.supplier.created_on <= request.create_date_to.Value);
            }

            if (request.status_flag is not null && request.status_flag.Count > 0)
            {
                supplierMainQuery = supplierMainQuery.Where(x => request.status_flag.Contains(x.supplier.status_flag));
            }

            // filtered by the Keyword (supplier_no and supplier_name)
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                supplierMainQuery = supplierMainQuery.Where(x =>
                    x.supplier.supplier_no.Contains(request.Keyword) ||
                    x.supplier.supplier_name.Contains(request.Keyword));
            }

            totalRows = supplierMainQuery.Select(x => x.supplier.id).Count();

            var result = from mainQuery in supplierMainQuery
                         join defaultCurrency in _dbContext.Currency on mainQuery.supplier.default_currency_id equals defaultCurrency.id into currencyGroup
                         from defaultCurrency in currencyGroup.DefaultIfEmpty()
                         join defaultCountryOfLoading in _dbContext.Country on mainQuery.supplier.default_country_of_loading_id equals
                             defaultCountryOfLoading.id into countryOfLoadingGroup
                         from defaultCountryOfLoading in countryOfLoadingGroup.DefaultIfEmpty()
                         join defaultPortOfLoading in _dbContext.Ports on mainQuery.supplier.default_port_of_loading_id equals
                             defaultPortOfLoading.id into portOfLoadingGroup
                         from defaultPortOfLoading in portOfLoadingGroup.DefaultIfEmpty()
                         join defaultCountryOfDischarge in _dbContext.Country on mainQuery.supplier.default_country_of_discharge_id equals
                             defaultCountryOfDischarge.id into countryOfDischargeGroup
                         from defaultCountryOfDischarge in countryOfDischargeGroup.DefaultIfEmpty()
                         join defaultPortOfDischarge in _dbContext.Ports on mainQuery.supplier.default_port_of_discharge_id equals
                             defaultPortOfDischarge.id into portOfDischargeGroup
                         from defaultPortOfDischarge in portOfDischargeGroup.DefaultIfEmpty()
                         select new PagedSupplierDetail
                         {
                             id = mainQuery.supplier.id,
                             supplier_no = mainQuery.supplier.supplier_no,
                             supplier_name = mainQuery.supplier.supplier_name,
                             registered_site_id = mainQuery.supplier.registered_site_id,
                             registered_site_no = mainQuery.register_site_no,
                             registered_site_name = mainQuery.register_site_name,
                             status_flag = mainQuery.supplier.status_flag,
                             service_flag = mainQuery.supplier.service_flag,
                             product_flag = mainQuery.supplier.product_flag,
                             authorised_flag = mainQuery.supplier.authorised_flag,
                             payment_term = mainQuery.supplier.payment_term,
                             payment_method = mainQuery.supplier.payment_method,
                             default_currency_id = mainQuery.supplier.default_currency_id,
                             default_currency = defaultCurrency.currency_code,
                             landed_cost_rule = mainQuery.supplier.landed_cost_rule,
                             incoterm = mainQuery.supplier.incoterm,
                             default_freight_method = mainQuery.supplier.default_freight_method,
                             po_sending_method = mainQuery.supplier.po_sending_method,
                             default_country_of_loading = defaultCountryOfLoading == null
                                 ? null
                                 : new CountryDetail
                                 {
                                     country_id = defaultCountryOfLoading.id,
                                     country_name = defaultCountryOfLoading.country_name,
                                     country_long_name = defaultCountryOfLoading.country_long_name,
                                     country_alpha_code_two = defaultCountryOfLoading.country_alpha_code_two,
                                     country_alpha_code_three = defaultCountryOfLoading.country_alpha_code_three,
                                     country_idd = defaultCountryOfLoading.country_idd,
                                     continent = defaultCountryOfLoading.continent
                                 },
                             default_port_of_loading = defaultPortOfLoading == null
                                 ? null
                                 : new PortInfoDetail
                                 {
                                     port_id = defaultPortOfLoading.id,
                                     port_no = defaultPortOfLoading.port_no,
                                     port_name = defaultPortOfLoading.port_name
                                 },
                             default_country_of_discharge = defaultCountryOfDischarge == null
                                 ? null
                                 : new CountryDetail
                                 {
                                     country_id = defaultCountryOfDischarge.id,
                                     country_name = defaultCountryOfDischarge.country_name,
                                     country_long_name = defaultCountryOfDischarge.country_long_name,
                                     country_alpha_code_two = defaultCountryOfDischarge.country_alpha_code_two,
                                     country_alpha_code_three = defaultCountryOfDischarge.country_alpha_code_three,
                                     country_idd = defaultCountryOfDischarge.country_idd,
                                     continent = defaultCountryOfDischarge.continent
                                 },
                             default_port_of_discharge = defaultPortOfDischarge == null
                                 ? null
                                 : new PortInfoDetail
                                 {
                                     port_id = defaultPortOfDischarge.id,
                                     port_no = defaultPortOfDischarge.port_no,
                                     port_name = defaultPortOfDischarge.port_name
                                 },
                             created_on = mainQuery.supplier.created_on,
                             created_by = mainQuery.supplier.created_by,
                             last_modified_by = mainQuery.supplier.last_modified_by,
                             last_modified_on = mainQuery.supplier.last_modified_on
                         };

            return result;
        }

        public async Task<string[]> CheckInvalidSupplierNOs(List<Supplier> suppliers)
        {
            var supplierQuery = _dbContext.Supplier.AsQueryable();

            var parameter = Expression.Parameter(typeof(Supplier), "x");
            Expression predicate = Expression.Constant(false);

            foreach (var condition in suppliers)
            {
                var supplierIdCondition = Expression.NotEqual(
                    Expression.Property(parameter, "id"),
                    Expression.Constant(condition.id)
                );

                var supplierNoCondition = Expression.Equal(
                    Expression.Property(parameter, "supplier_no"),
                    Expression.Constant(condition.supplier_no)
                );

                var combinedCondition = Expression.AndAlso(supplierIdCondition, supplierNoCondition);
                predicate = Expression.OrElse(predicate, combinedCondition);
            }

            var lambda = Expression.Lambda<Func<Supplier, bool>>(predicate, parameter);
            var supplierNo = await supplierQuery.Where(lambda)
                .Select(x => x.supplier_no).ToArrayAsync();

            return supplierNo;
        }

        public async Task<string[]> CheckExistingSupplierNo(List<string> supplierNOs)
        {
            var supplierQuery = _dbContext.Supplier.Where(x => supplierNOs.Contains(x.supplier_no));

            var existingSupplierNo = await supplierQuery.Select(x => x.supplier_no).ToArrayAsync();
            return existingSupplierNo;
        }
    }
}
