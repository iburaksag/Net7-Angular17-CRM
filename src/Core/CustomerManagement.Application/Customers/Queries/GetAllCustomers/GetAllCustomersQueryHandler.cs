using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Customers.Queries.GetAllCustomers
{
	public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, ServiceResponse<List<CustomerDto>>>
	{
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCustomersQueryHandler> _logger;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetAllCustomersQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllCustomersByCreatedDateAsync();

            if (customers is null)
            {
                var error = "Customer list is null.";
                _logger.LogError("GetAllCustomersQueryHandler returned null value.");
                return ServiceResponse<List<CustomerDto>>.Fail(new List<string> { error });
            }

            var customersDto = _mapper.Map<List<CustomerDto>>(customers);

            _logger.LogInformation("GetAllCustomersQuery handled successfully.");
            return new ServiceResponse<List<CustomerDto>> { IsSuccess = true, Data = customersDto };
        }
    }
}

