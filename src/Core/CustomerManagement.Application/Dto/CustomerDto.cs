namespace CustomerManagement.Application.Dto
{
    public record CustomerDto(
                    Guid Id,
                    String FirstName,
                    String LastName,
                    String Email,
                    String Phone,
                    String Address,
                    String City,
                    String Country);
}

