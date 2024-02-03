using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
	public sealed record UpdateCustomerCommand(
                Guid Id,
                String FirstName,
                String LastName,
                String Email,
                String Phone,
                String Address,
                String City,
                String Country,
                Guid UserId,
                DateTime UpdatedDate) : IRequest<ServiceResponse<CustomerDto>>;
}

