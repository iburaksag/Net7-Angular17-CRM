using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Customers.Queries.GetCustomerById
{
    public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<ServiceResponse<CustomerDto>>;

}

