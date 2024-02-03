using System;
using CustomerManagement.Application;
using CustomerManagement.Application.Users.Commands.Login;
using Newtonsoft.Json;
using System.Net;
using CustomerManagement.Persistance.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using FluentAssertions;
using CustomerManagement.Application.Dto;
using CustomerManagement.Application.Users.Commands.Register;

namespace CustomerManagement.Api.IntegrationTests.Controllers
{
	public class UsersControllerTests
	{
		public UsersControllerTests()
		{
		}

        [Fact]
        public async Task OnRegisterUser_WhenExecuteApi_ShouldReturnResponseWithNewlyCreatedUserData()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            // Act
            var registerRequest = new RegisterCommand(
                Username: "TestUser",
                Email: "testuser@example.com",
                Password: "TestPassword123",
                FirstName: "Test",
                LastName: "User");

            var registerResponse = await client.PostAsJsonAsync("/api/v1/user/register", registerRequest);
            var content = await registerResponse.Content.ReadAsStringAsync();

            // Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            registerResponse.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UserDto>>(content);
            serviceResponse.Should().NotBeNull();
            serviceResponse.IsSuccess.Should().BeTrue();
            serviceResponse.Errors.Should().BeNull();
            serviceResponse.Data.Should().NotBeNull();
            serviceResponse.Data.Username.Should().Be(registerRequest.Username);
            serviceResponse.Data.Email.Should().Be(registerRequest.Email);
        }


        [Fact]
        public async Task OnLoginUser_WhenExecuteApi_ShouldReturnResponseWithToken()
        {
            //Arrange
            var factory = new CustomWebApplicationFactory();

            using (var scope = factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.SaveChanges();
            }

            var client = factory.CreateClient();

            // Act
            var loginRequest = new LoginCommand(
                    Email: "testuser@test.com",
                    Password: "Test1234");

            var loginResponse = await client.PostAsJsonAsync("/api/v1/user/login", loginRequest);
            var content = await loginResponse.Content.ReadAsStringAsync();

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            loginResponse.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var authResult = JsonConvert.DeserializeObject<AuthResult>(content);
            authResult.Should().NotBeNull();
            authResult.Success.Should().BeTrue();
            authResult.Token.Should().NotBeNullOrEmpty();
            authResult.Message.Should().BeNull();
            authResult.Errors.Should().BeNull();
        }

    }
}

