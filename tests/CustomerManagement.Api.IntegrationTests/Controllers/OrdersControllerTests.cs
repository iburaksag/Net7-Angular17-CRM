using CustomerManagement.Application;
using CustomerManagement.Application.Customers.Commands.CreateCustomer;
using CustomerManagement.Application.Dto;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Persistance.Context;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace CustomerManagement.Api.IntegrationTests.Controllers
{
    public class OrdersControllerTests
    {
        public OrdersControllerTests()
        {
        }

        [Fact]
        public async void OnGetAllOrders_WhenExecuteApi_ShouldReturnResponseWithOrderList()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/order");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<OrderDto>>>();

            // Assert
            response.EnsureSuccessStatusCode();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.Data.Should().NotBeNull();
        }


        [Fact]
        public async Task OnGetOrdersByCustomerId_WhenExecuteApi_ShouldReturnResponseWithOrderListForCustomer()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();


            // Test customer
            var createCustomerRequest = new CreateCustomerCommand(
                        FirstName: "Test",
                        LastName: "Test",
                        Email: "test@gmail.com",
                        Phone: "12345678",
                        Address: "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestestTestTestTestTestTestTestTestTestTest",
                        City: "Test",
                        Country: "Test",
                        UserId: Guid.Empty);

            var createCustomerResponse = await client.PostAsJsonAsync("/api/v1/customer", createCustomerRequest);
            createCustomerResponse.EnsureSuccessStatusCode();

            var createdCustomer = await createCustomerResponse.Content.ReadFromJsonAsync<ServiceResponse<CustomerDto>>();
            var customerId = createdCustomer.Data.Id;

            // Act
            var response = await client.GetAsync($"/api/v1/order/customer/{customerId}");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var result = JsonConvert.DeserializeObject<ServiceResponse<List<OrderDto>>>(content);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.Data.Should().NotBeNull();
            
            result.Data.Should().OnlyContain(order => order.CustomerId == customerId);

        }
    }
}

