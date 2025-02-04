using Microsoft.AspNetCore.Http;
using SERP.Application.Common.Dto;
using SERP.Application.Masters.Suppliers.DTOs;
using SERP.Application.Masters.Suppliers.DTOs.Request;
using SERP.Application.Masters.Suppliers.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Masters.Suppliers.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierLimitedDto>> GetAllLimited(bool onlyEnabled);
        Task<SupplierDto> GetById(int id);
        Task<List<SecondarySupplierLimitedDto>> GetSecondarySupplierLimited(int supplierId, bool onlyEnabled);
        Task<List<SupplierItemMappingDto>> GetSupplierItemMapping(int supplierId, int itemId, bool onlyEnabled);
        Task<object> ImportSupplierAsync(string userId, IFormFile file);
        Task<object> ImportSupplierItemAsync(string userId, IFormFile file);
        Task<object> ImportSupplierSecondaryAsync(string userId, IFormFile file);
        Task<int[]> CreateSupplierAsync(string userId, List<CreateSupplierDto> requests);
        Task UpdateSupplierAsync(string userId, List<UpdateSupplierDto> requests);
        Task DeleteSupplierAsync(List<int> supplierIDs);
        Task<PagedResponse<SupplierPagedResponseDto>> SearchPagedAsync(SearchPagedRequestDto request, FilterPagedSupplierRequestDto filter);
        Task<PagedResponse<SupplierItemMappingPagedResponseDto>> SearchItemMappingPagedAsync(SearchPagedRequestDto request, FilterPagedRelativeSupplierRequestDto filter);
        Task<PagedResponse<SecondarySupplierPagedResponseDto>> SearchSecondaryPagedAsync(SearchPagedRequestDto request, FilterPagedRelativeSupplierRequestDto filter);
        Task<PagedResponse<IntermediarySupplierPagedResponseDto>> SearchIntermediaryPagedAsync(SearchPagedRequestDto request, FilterPagedRelativeSupplierRequestDto filter);
        Task<PagedResponse<SupplierSelfCollectSitePagedResponseDto>> SearchSelfCollectSitePagedAsync(SearchPagedRequestDto request, FilterPagedSupplierSelfCollectSiteRequestDto filter);
        Task UpdateItemMappingAsync(string userId, int supplierId, List<UpdateItemMappingRequestDto> request);
        Task UpdateSecondaryAsync(string userId, int supplierId, List<UpdateSecondarySupplierRequestDto> request);
        Task UpdateIntermediaryAsync(string userId, int supplierId, List<UpdateIntermediaryRequestDto> request);
        Task UpdateSelfCollectSiteAsync(string userId, int supplierId, List<UpdateSupplierSelfCollectSiteRequestDto> request);
    }
}
