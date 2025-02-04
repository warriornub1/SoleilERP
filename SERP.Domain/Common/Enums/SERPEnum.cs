using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Domain.Common.Enums
{
    public static class SERPEnum
    {
        public enum SupplierFields
        {
            SupplierNo = 0,
            SupplierName = 1
        }

        public enum SiteFields
        {
            SiteNo = 0,
            SiteName = 1
        }

        public enum PortFields
        {
            PortNo = 0,
            PortName = 1
        }

        public enum CurrencyFields
        {
            Currency = 0,
            StatusFlag = 1
        }

        public enum AgentFields
        {
            AgentNo = 0,
            AgentName = 1
        }

        public enum BranchPlantFields
        {
            BranchPlantNo = 0,
            BranchPlantName = 1
        }

        public enum CompanyFields
        {
            CompanyNo = 0,
            CompanyName = 1
        }

        public enum ItemFields
        {
            ItemNo = 0,
            Description1 = 1,
            Description2 = 2,
            PrimaryUom = 3,
            SecondaryUom = 4,
        }

        public enum SupplierItemMappingFields
        {
            SupplierPartNo = 0,
            SupplierMaterialCode = 1,
            SupplierMaterialDescription = 2
        }

        public enum CountryFields
        {
            CountryCodeTwo = 0,
            CountryName = 1
        }

        public enum DictionaryType
        {
            Supplier,
            SecondarySupplier,
            Site,
            Port,
            Currency,
            Agent,
            BranchPlant,
            Company,
            Item,
            SupplierItemMapping,
            Country
        }

        public enum Mode
        {
            Insert,
            Update
        }
    }
}
