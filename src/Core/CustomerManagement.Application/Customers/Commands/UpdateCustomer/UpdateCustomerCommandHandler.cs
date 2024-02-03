using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Domain.Repositories.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
	public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ServiceResponse<CustomerDto>>
	{
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<UpdateCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResponse<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new UpdateCustomerCommandValidator().ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                _logger.LogError("Validation failed while updating customer. {errors}", errors);
                return ServiceResponse<CustomerDto>.Fail(errors);
            }

            var existingCustomer = await _customerRepository.GetByIdAsync(request.Id);

            if (existingCustomer is null)
            {
                var error = "Customer is not exist.";
                _logger.LogError("Customer with Id {request.Id} is not exist.", request.Id);
                return ServiceResponse<CustomerDto>.Fail(new List<string> { error });
            }

            existingCustomer = _mapper.Map(request, existingCustomer);

            await _customerRepository.UpdateAsync(existingCustomer);
            await _unitOfWork.SaveChangesAsync();

            var updatedCustomerDto = _mapper.Map<CustomerDto>(existingCustomer);

            _logger.LogInformation("Customer is updated successfully. {FirstName} {LastName}", request.FirstName, request.LastName);
            return new ServiceResponse<CustomerDto> { IsSuccess = true, Data = updatedCustomerDto };
        }
    }
}

