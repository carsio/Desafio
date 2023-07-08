using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;


namespace Tests.Integration;

public class ExceptionFilterTest : IntegrationTests {
    public ExceptionFilterTest(CustomWebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Error_ReturnsInternalServerErrorStatusCode() {
        // Act
        var response = await _client.GetAsync("/api/auth/error");
        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("Server Error", await response.Content.ReadAsStringAsync());
    }
}