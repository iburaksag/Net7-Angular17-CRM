using CustomerManagement.Domain.Common;
using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Domain.Entities
{
	public class Order : BaseEntity
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public Status Status { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}

