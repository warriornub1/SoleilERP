using SERP.Application.Common;
using SERP.Domain.Masters.Customers;
using SERP.Domain.Masters.Customers.Models;

namespace SERP.Application.Masters.Customers.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllLimited(bool onlyEnabled);
        Task<IEnumerable<CustomerShipToDetail>> GetAllShipToByCustomer(int customerId, bool onlyEnabled);
    }
}
