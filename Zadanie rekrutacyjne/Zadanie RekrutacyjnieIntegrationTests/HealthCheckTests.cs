using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace Zadanie_RekrutacyjnieIntegrationTests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public HealthCheckTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_ShouldReturnOkStatus()
        {
            var response = await _httpClient.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }
    }
}
