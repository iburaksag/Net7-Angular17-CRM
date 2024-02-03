using CustomerManagement.Application.Orders.Queries.GetAllOrders;
using CustomerManagement.Application.Orders.Queries.GetOrdersByCustomerid;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/order")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Getting order list of specific customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has list of orders belongs to specific customer</returns>
        /// <response code="200">Returns response object which has data of order list for specific customer</response>
        /// <response code="404">If order list for specific customer returns null</response>  
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(Guid customerId)
        {
            try
            {
                var query = new GetOrdersByCustomerIdQuery(customerId);
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                {
                    return NotFound(new { Errors = result.Errors });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Getting a list of orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has list of order</returns>
        /// <response code="200">Returns response object which has data of order list</response>
        /// <response code="404">If order list returns null</response>  
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var query = new GetAllOrdersQuery();
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                {
                    return NotFound(new { Errors = result.Errors });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

