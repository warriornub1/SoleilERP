using Microsoft.AspNetCore.Http;
using SERP.Application.Masters.Items.DTOs;

namespace SERP.Application.Masters.Items.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemLimitedDto>> GetAllLimited(bool onlyEnabled);

        Task<ItemDto> GetById(int id);

        Task<object> ImportItemAsync(string userId, IFormFile file);
    }
}
