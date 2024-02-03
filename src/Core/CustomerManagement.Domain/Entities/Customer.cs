using CustomerManagement.Domain.Common;

namespace CustomerManagement.Domain.Entities
{
	public class Customer : BaseEntity
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; }

        public String Address { get; set; }

        public String City { get; set; }

        public String Country { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

