using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ServiceResponse<List<OrderDto>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllOrdersQueryHandler> _logger;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetAllOrdersQueryHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders is null)
            {
                var error = "Order list is null.";
                _logger.LogError("GetAllOrdersQueryHandler returned null value.");
                return ServiceResponse<List<OrderDto>>.Fail(new List<string> { error });
            }

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
        
            _logger.LogInformation("GetAllOrdersQuery handled successfully.");
            return new ServiceResponse<List<OrderDto>> { IsSuccess = true, Data = ordersDto };
        }
    }
}

