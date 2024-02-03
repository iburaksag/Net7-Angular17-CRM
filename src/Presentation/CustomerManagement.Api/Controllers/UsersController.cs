using CustomerManagement.Application.Users.Commands.Login;
using CustomerManagement.Application.Users.Commands.Register;
using CustomerManagement.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Login process for the user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/v1/user/login
        ///     {        
        ///       "email": "buraksag@gmail.com"
        ///       "password": "123456"
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns authResult object which has user data</returns>
        /// <response code="200">Returns authResult object which has user data successfully</response>
        /// <response code="401">If invalid credentials or user can not be found</response>    
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> LoginUser([FromBody] LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var authResult = await _mediator.Send(request, cancellationToken);

                if (!authResult.Success)
                {
                    return Unauthorized(new { Message = authResult.Message, Errors = authResult.Errors });
                }

                return Ok(authResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        /// <summary>
        /// Creating a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/v1/user/register
        ///     {        
        ///       "username": "buraksag"
        ///       "email": "buraksag@gmail.com"
        ///       "password": 123456
        ///       "firstName": Burak
        ///       "lastName": Sag
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has new created user data</returns>
        /// <response code="200">Returns response object which has new created user data successfully</response>
        /// <response code="400">If user is already exist or validation error</response>    
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(request, cancellationToken);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { Errors = result.Errors });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get user model by sending user id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns response object which has user data</returns>
        /// <response code="200">Returns response object which has user data successfully</response>
        /// <response code="404">If user is not found</response>    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdQuery(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery(id);
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

