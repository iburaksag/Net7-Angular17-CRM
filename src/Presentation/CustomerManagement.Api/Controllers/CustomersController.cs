using System.Security.Claims;
using CustomerManagement.Application.Customers.Commands.CreateCustomer;
using CustomerManagement.Application.Customers.Commands.DeleteCustomer;
using CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using CustomerManagement.Application.Customers.Queries.GetAllCustomers;
using CustomerManagement.Application.Customers.Queries.GetCustomerById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/customer")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Getting a list of customers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has list of customers</returns>
        /// <response code="200">Returns response object which has data of customer list</response>
        /// <response code="404">If customer list returns null</response>  
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var query = new GetAllCustomersQuery();
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
        /// Get customer model by sending customer id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has customer data</returns>
        /// <response code="200">Returns response object which has customer data successfully</response>
        /// <response code="404">If customer is not found</response>    
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerByIdQuery(Guid customerId)
        {
            try
            {
                var query = new GetCustomerByIdQuery(customerId);
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
        /// Creating a new customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///         POST api/v1/customer
        ///         {
        ///             "firstName": "Burak",
        ///             "lastName": "Sag",
        ///             "email": "iburaksag@gmail.com",
        ///             "phone": "05395395339",
        ///             "address": "Libadiye cd. Teknik sk. Uzunlar sit...",
        ///             "city": "Istanbul",
        ///             "country": "Turkiye",
        ///             "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///         }
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has newly created customer data</returns>
        /// <response code="200">Returns response object which has newly created customer data successfully</response>
        /// <response code="401">If customer is already exist or validation error</response>  
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim is null)
                {
                    return Unauthorized("User ID not found in the token.");
                }

                if (Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    command = command with { UserId = userId };
                }
                else
                {
                    return BadRequest("Invalid format for User ID in the token.");
                }

                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { Errors = result.Errors });
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        /// <summary>
        /// Updating existing customer with new data
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///         PUT api/v1/customer/{customerId}
        ///         {
        ///             "firstName": "Burak",
        ///             "lastName": "Sag",
        ///             "email": "iburaksag@gmail.com",
        ///             "phone": "05395395339",
        ///             "address": "Libadiye cd. Teknik sk. Uzunlar sit...",
        ///             "city": "Istanbul",
        ///             "country": "Turkiye",
        ///             "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///             "updatedDate": "2024-01-13T20:35:26.001Z"
        ///         }
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has newly updated customer data</returns>
        /// <response code="200">Returns response object which has newly updated customer data successfully</response>
        /// <response code="400">If customer is not exist or validation error</response> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim is null)
                    return Unauthorized("User ID not found in the token.");

                if (Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    command = command with { Id = id, UserId = userId, UpdatedDate = DateTime.UtcNow };
                }
                else
                {
                    return BadRequest("Invalid format for User ID in the token.");
                }

                var result = await _mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { Errors = result.Errors });
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        /// <summary>
        /// Deleting specific customer by sending customer id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns a OkResult object that produces an empty response</returns>
        /// <response code="200">Returns a OkResult object that produces an empty response successfully</response>
        /// <response code="400">If customer is not exist</response> 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteCustomerCommand(id));

                if (!result.IsSuccess)
                {
                    return BadRequest(new { Errors = result.Errors });
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}

