using MediatR;

namespace CustomerManagement.Application.Customers.Commands.DeleteCustomer
{
    public sealed record DeleteCustomerCommand(Guid Id) : IRequest<ServiceResponse<Unit>>;
}

