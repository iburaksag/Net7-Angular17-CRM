using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Repositories;
using CustomerManagement.Domain.Repositories.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Application.Users.Commands.Register
{
	public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ServiceResponse<UserDto>>
	{
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public RegisterCommandHandler(
            IUserRepository userRepository,
            ILogger<RegisterCommandHandler> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await new RegisterCommandValidator().ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                _logger.LogError("Validation failed. {errors}", errors);
                return ServiceResponse<UserDto>.Fail(errors);
            }

            if (await _userRepository.IsUsernameTakenAsync(request.Username))
            {
                var error = "Username is already taken.";
                _logger.LogError("{Username} Username is already taken", request.Username);
                return ServiceResponse<UserDto>.Fail(new List<string> { error });
            }

            if (await _userRepository.IsEmailTakenAsync(request.Email))
            {
                var error = "Email is already taken.";
                _logger.LogError("{Email} Email is already taken", request.Email);
                return ServiceResponse<UserDto>.Fail(new List<string> { error });
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);

            _logger.LogInformation("RegisterCommand handled successfully.");
            return new ServiceResponse<UserDto> { IsSuccess = true, Data = userDto };
        }

        //Hashing
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}

