using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Abstractions
{
	public interface IJwtProvider
	{
		string Generate(User user);
	}
}

