namespace CustomerManagement.Application.Dto
{
	public record OrderDto(
                    Guid Id,
                    String Name,
                    String Description,
                    decimal TotalAmount,
                    Guid CustomerId);
}


