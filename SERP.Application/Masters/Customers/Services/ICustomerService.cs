using SERP.Application.Masters.Customers.DTOs;

namespace SERP.Application.Masters.Customers.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerLimitedDto>> GetAllLimited(bool onlyEnabled);
        Task<List<CustomerShipToDetailDto>> GetAllShipToByCustomer(int customerId, bool onlyEnabled);
    }
}
