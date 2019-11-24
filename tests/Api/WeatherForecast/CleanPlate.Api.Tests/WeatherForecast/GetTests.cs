using CleanPlate.WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace CleanPlate.Api.Tests
{
    public class GetTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public GetTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void Get_ShouldReturnSuccess_WhenCalled()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/weatherforecast");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void Get_ShouldReturnFiveElementContent_WhenCalled()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/weatherforecast");
            var content = await response.Content.ReadAsStringAsync();
            var forecast = JsonSerializer.Deserialize<WeatherForecast[]>(content);

            // Assert
            Assert.Equal(5, forecast.Length);
        }
    }
}
