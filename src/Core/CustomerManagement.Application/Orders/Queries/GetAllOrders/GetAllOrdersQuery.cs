using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Orders.Queries.GetAllOrders
{
    public sealed record GetAllOrdersQuery() : IRequest<ServiceResponse<List<OrderDto>>>;
}

