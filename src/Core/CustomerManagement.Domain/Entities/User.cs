using CustomerManagement.Domain.Common;

namespace CustomerManagement.Domain.Entities
{
	public class User : BaseEntity
	{
        public String Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public String Email { get; set; }

        public String? FirstName { get; set; }

        public String? LastName { get; set; }
    }
}

