using AutoMapper;
using SERP.Application.Masters.Customers.DTOs;
using SERP.Application.Masters.Customers.Interfaces;

namespace SERP.Application.Masters.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerLimitedDto>> GetAllLimited(bool onlyEnabled)
        {
            return _mapper.Map<List<CustomerLimitedDto>>(await _customerRepository.GetAllLimited(onlyEnabled));
        }

        public async Task<List<CustomerShipToDetailDto>> GetAllShipToByCustomer(int customerId, bool onlyEnabled)
        {
            return _mapper.Map<List<CustomerShipToDetailDto>>(await _customerRepository.GetAllShipToByCustomer(customerId, onlyEnabled));
        }
    }
}
