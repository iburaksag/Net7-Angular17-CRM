using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Customers.Commands.CreateCustomer
{
    public sealed record CreateCustomerCommand(
                String FirstName,
                String LastName,
                String Email,
                String Phone,
                String Address,
                String City,
                String Country,
                Guid UserId) : IRequest<ServiceResponse<CustomerDto>>;
}

