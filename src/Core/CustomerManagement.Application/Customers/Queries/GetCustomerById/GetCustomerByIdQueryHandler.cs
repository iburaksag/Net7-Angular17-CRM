using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ServiceResponse<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetCustomerByIdQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer is null)
            {
                var error = "Customer is null.";
                _logger.LogError("GetCustomerByIdQueryHandler returned null value for Customer id {id}", request.Id);
                return ServiceResponse<CustomerDto>.Fail(new List<string> { error });
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            _logger.LogInformation("GetCustomerByIdQueryHandler handled successfully.");
            return new ServiceResponse<CustomerDto> { IsSuccess = true, Data = customerDto };
        }
    }
}

