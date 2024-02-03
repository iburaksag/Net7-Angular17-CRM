using System.Security.Cryptography;
using System.Text;
using CustomerManagement.Application.Abstractions;
using CustomerManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomerManagement.Application.Users.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, ILogger<LoginCommandHandler> logger)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _logger = logger;
        }

        public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //VALIDATION
            var validationResult = await new LoginCommandValidator().ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                _logger.LogError("Validation failed.");
                return new AuthResult { Success = false, Message = "Validation failed.", Errors = errors };
            }

            //GET USER
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                var error = "User not found.";
                _logger.LogError("User not found.");
                return new AuthResult { Success = false, Errors = new List<string> { error } };
            }

            if (!VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                var error = "Invalid email or password.";
                _logger.LogError("Invalid email or password. {Email} / {Password}", request.Email, request.Password);
                return new AuthResult { Success = false, Errors = new List<string> { error } };
            }

            //GENERATE JWT
            string token = _jwtProvider.Generate(user);

            //RETURN JWT
            _logger.LogInformation("LoginCommand handled successfully.");
            return new AuthResult { Success = true, Token = token };

        }

        //Verify
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
    }
}

