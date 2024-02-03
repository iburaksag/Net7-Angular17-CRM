using AutoMapper;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Domain.Repositories.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Customers.Commands.DeleteCustomer
{
	public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ServiceResponse<Unit>>
	{
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;

        public DeleteCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResponse<Unit>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerToDelete = await _customerRepository.GetByIdAsync(request.Id);

            if (customerToDelete is null)
            {
                var error = "Customer which trying to be deleted not exist.";
                _logger.LogError("Customer with {Id} is not exist", request.Id);
                return ServiceResponse<Unit>.Fail(new List<string> { error });
            }

            await _customerRepository.DeleteAsync(customerToDelete);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Customer is deleted successfully.");
            return new ServiceResponse<Unit> { IsSuccess = true, Data = Unit.Value };
        }
    }
}

