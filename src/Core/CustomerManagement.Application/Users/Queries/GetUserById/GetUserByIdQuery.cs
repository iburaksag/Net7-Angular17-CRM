using CustomerManagement.Application.Dto;
using MediatR;

namespace CustomerManagement.Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(Guid Id) : IRequest<ServiceResponse<UserDto>>;

}

