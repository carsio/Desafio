using System.Text;
using System.Text.Json;
using System.Net;
using Core.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;


namespace Tests.Integration;

public class UserTest : IntegrationTests {
    public UserTest(CustomWebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Register_ReturnsSuccessStatusCode() {
        var uniqueUser = Guid.NewGuid().ToString();
        // Arrange
        var userDto = new CreateUserDto {
            Email = $"{uniqueUser}@test.com",
            Password = "fake_password"
        };
        // Act
        var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/register", content);
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }

    [Fact]
    public async Task Register_ReturnsBadRequestStatusCode() {
        // Arrange
        var userDto = new CreateUserDto {
            Email = "test@test.com",
            Password = "fake_password"
        };
        var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/auth/register", content);
        // Act
        var response = await _client.PostAsync("/api/auth/register", content);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("User already exists", await response.Content.ReadAsStringAsync());
    }
}