using MediatR;

namespace CustomerManagement.Application.Users.Commands.Login
{
	public record LoginCommand(String Email, String Password) : IRequest<AuthResult>;
}

