using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Orders.Queries.GetOrdersByCustomerid
{
	public class GetOrdersByCustomerIdQueryHandler : IRequestHandler<GetOrdersByCustomerIdQuery, ServiceResponse<List<OrderDto>>>
	{
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrdersByCustomerIdQueryHandler> _logger;

        public GetOrdersByCustomerIdQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrdersByCustomerIdQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<OrderDto>>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdWithOrderAsync(request.CustomerId);

            if (orders.Count == 0)
            {
                var error = "This customer does not have any orders.";
                _logger.LogError("GetOrdersByCustomerIdQuery returned null value for Customer id {id}", request.CustomerId);
                return ServiceResponse<List<OrderDto>>.Fail(new List<string> { error });
            }

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);

            _logger.LogInformation("GetOrdersByCustomerIdQuery handled successfully.");
            return new ServiceResponse<List<OrderDto>> { IsSuccess = true, Data = ordersDto };

        }
    }
}

