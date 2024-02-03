namespace CustomerManagement.Application.Dto
{
    public record UserDto(
                    Guid Id,
                    String Username,
                    String Email,
                    String FirstName,
                    String LastName);
}

