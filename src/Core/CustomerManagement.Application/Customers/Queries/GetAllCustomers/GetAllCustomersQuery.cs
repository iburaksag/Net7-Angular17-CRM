using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Customers.Queries.GetAllCustomers
{
	public sealed record GetAllCustomersQuery() : IRequest<ServiceResponse<List<CustomerDto>>>;
}

