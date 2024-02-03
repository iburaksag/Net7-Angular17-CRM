using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Domain.Repositories.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Customers.Commands.CreateCustomer
{
	public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ServiceResponse<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCustomerCommandHandler> _logger;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<CreateCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResponse<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new CreateCustomerCommandValidator().ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                _logger.LogError("Validation failed while creating customer. {errors}", errors);
                return ServiceResponse<CustomerDto>.Fail(errors);
            }

            if (await _customerRepository.IsCustomerExistAsync(request.Email))
            {
                var error = "Customer is already exist.";
                _logger.LogError("Customer with {Email} is already exist", request.Email);
                return ServiceResponse<CustomerDto>.Fail(new List<string> { error });
            }

            var newCustomer = _mapper.Map<Customer>(request);

            await _customerRepository.AddAsync(newCustomer);
            await _unitOfWork.SaveChangesAsync();

            var createdCustomerDto = _mapper.Map<CustomerDto>(newCustomer);

            _logger.LogInformation("Customer is created successfully. {FirstName} {LastName}", request.FirstName, request.LastName);
            return new ServiceResponse<CustomerDto> { IsSuccess = true, Data = createdCustomerDto };
        }
    }
}

