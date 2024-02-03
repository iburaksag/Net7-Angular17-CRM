using System.Net;
using System.Net.Http.Json;
using CustomerManagement.Application;
using CustomerManagement.Application.Customers.Commands.CreateCustomer;
using CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using CustomerManagement.Application.Dto;
using CustomerManagement.Persistance.Context;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CustomerManagement.Api.IntegrationTests.Controllers
{
    public class CustomersControllerTests
    {
        public CustomersControllerTests()
        {
        }


        [Fact]
        public async void OnGetAllCustomers_WhenExecuteApi_ShouldReturnResponseWithCustomerList()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory();

            using(var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            //Act
            var response = await client.GetAsync("/api/v1/customer");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CustomerDto>>>();

            //Assert
            response.EnsureSuccessStatusCode();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeNull(); 
            result.Data.Should().NotBeNull();
        }


        [Fact]
        public async Task OnGetCustomerById_WhenExecuteApi_ShouldReturnResponseWithCustomer()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();
            
            var createCustomerRequest = new CreateCustomerCommand(
                        FirstName:"Test",
                        LastName:"Test",
                        Email:"test@gmail.com",
                        Phone:"12345678",
                        Address: "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestestTestTestTestTestTestTestTestTestTest",
                        City: "Test",
                        Country: "Test",
                        UserId: Guid.Empty);

            var createResponse = await client.PostAsJsonAsync("/api/v1/customer", createCustomerRequest);
            createResponse.EnsureSuccessStatusCode();

            var createdCustomer = await createResponse.Content.ReadFromJsonAsync<ServiceResponse<CustomerDto>>();
            var customerId = createdCustomer.Data.Id;

            // Act
            var response = await client.GetAsync($"/api/v1/customer/{customerId}");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var result = JsonConvert.DeserializeObject<ServiceResponse<CustomerDto>>(content);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.Data.Should().NotBeNull();

            result.Data.Id.Should().Be(createdCustomer.Data.Id);
            result.Data.FirstName.Should().Be(createdCustomer.Data.FirstName);
            result.Data.LastName.Should().Be(createdCustomer.Data.LastName);
        }


        [Fact]
        public async Task OnCreateCustomer_WhenExecuteApi_ShouldReturnResponseWithNewlyCreatedCustomer()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            var createCustomerRequest = new CreateCustomerCommand(
                        FirstName: "Test",
                        LastName: "Test",
                        Email: "test@gmail.com",
                        Phone: "12345678",
                        Address: "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestestTestTestTestTestTestTestTestTestTest",
                        City: "Test",
                        Country: "Test",
                        UserId: Guid.Empty);

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/customer", createCustomerRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var result = JsonConvert.DeserializeObject<ServiceResponse<CustomerDto>>(content);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.Data.Should().NotBeNull();

            result.Data.FirstName.Should().Be(createCustomerRequest.FirstName);
            result.Data.LastName.Should().Be(createCustomerRequest.LastName);
        }

        [Fact]
        public async Task OnUpdateCustomer_WhenExecuteApi_ShouldReturnResponseWithUpdatedCustomer()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            var createCustomerRequest = new CreateCustomerCommand(
                        FirstName: "Test",
                        LastName: "Test",
                        Email: "test@gmail.com",
                        Phone: "12345678",
                        Address: "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestestTestTestTestTestTestTestTestTestTest",
                        City: "Test",
                        Country: "Test",
                        UserId: Guid.Empty);

            var createResponse = await client.PostAsJsonAsync("/api/v1/customer", createCustomerRequest);
            createResponse.EnsureSuccessStatusCode();

            var createdCustomer = await createResponse.Content.ReadFromJsonAsync<ServiceResponse<CustomerDto>>();
            var customerId = createdCustomer.Data.Id;

            var updateCustomerRequest = new UpdateCustomerCommand(
                        Id: customerId,
                        FirstName: "Update",
                        LastName: "Update",
                        Email: "update@gmail.com",
                        Phone: "12345678",
                        Address: "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestestTestTestTestTestTestTestTestTestTest",
                        City: "Test",
                        Country: "Test",
                        UserId: Guid.Empty,
                        UpdatedDate: DateTime.Now);

            // Act
            var response = await client.PutAsJsonAsync($"/api/v1/customer/{customerId}", updateCustomerRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var result = JsonConvert.DeserializeObject<ServiceResponse<CustomerDto>>(content);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeNull();
            result.Data.Should().NotBeNull();

            result.Data.Id.Should().Be(customerId);
            result.Data.FirstName.Should().Be(updateCustomerRequest.FirstName);
            result.Data.LastName.Should().Be(updateCustomerRequest.LastName);
        }


        [Fact]
        public async Task OnDeleteCustomer_WhenExecuteApi_ShouldReturnResponseOK()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            var createCustomerRequest = new CreateCustomerCommand(
                        FirstName: "Test",
                        LastName: "Test",
                        Email: "test@gmail.com",
                        Phone: "12345678",
                        Address: "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestestTestTestTestTestTestTestTestTestTest",
                        City: "Test",
                        Country: "Test",
                        UserId: Guid.Empty);

            var createResponse = await client.PostAsJsonAsync("/api/v1/customer", createCustomerRequest);
            createResponse.EnsureSuccessStatusCode();

            var createdCustomer = await createResponse.Content.ReadFromJsonAsync<ServiceResponse<CustomerDto>>();
            var customerId = createdCustomer.Data.Id;

            // Act
            var response = await client.DeleteAsync($"/api/v1/customer/{customerId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Checking
            var getResponse = await client.GetAsync($"/api/v1/customer/{customerId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}

