using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Orders.Queries.GetOrdersByCustomerid
{
    public sealed record GetOrdersByCustomerIdQuery(Guid CustomerId) : IRequest<ServiceResponse<List<OrderDto>>>;
}

