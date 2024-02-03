using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Users.Commands.Register
{
    public record RegisterCommand(
                String Username,
                String Email,
                String Password,
                String? FirstName,
                String? LastName) : IRequest<ServiceResponse<UserDto>>;

}

