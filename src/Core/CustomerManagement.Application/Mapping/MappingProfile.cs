using AutoMapper;
using CustomerManagement.Application.Customers.Commands.CreateCustomer;
using CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<Customer, CreateCustomerCommand>().ReverseMap();
            CreateMap<Customer, UpdateCustomerCommand>().ReverseMap();
        }
    }
}

