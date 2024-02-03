using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Users.Queries.GetUserById
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ServiceResponse<UserDto>>
	{
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                var error = "User is null.";
                _logger.LogError("GetUserByIdQueryHandler returned null value for User id {id}", request.Id);
                return ServiceResponse<UserDto>.Fail(new List<string> { error });
            }

            var userDto = _mapper.Map<UserDto>(user);

            _logger.LogInformation("GetUserByIdQuery handled successfully.");
            return new ServiceResponse<UserDto> { IsSuccess = true, Data = userDto };
        }
    }
}

