using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Mocks;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_RekrutacyjnieIntegrationTests.Controllers
{
    public class TagsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _webApplicationFactory;

        public TagsControllerTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateDefaultClient();
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory]
        [InlineData("/api/tags?page=1&sortBy=name&order=asc")]
        [InlineData("/api/tags?page=1&sortBy=name&order=desc")]
        [InlineData("/api/tags?page=1&sortBy=share&order=asc")]
        [InlineData("/api/tags?page=1&sortBy=share&order=desc")]
        [InlineData("/api/tags?page=2&sortBy=name&order=asc")]
        [InlineData("/api/tags?page=2&sortBy=name&order=desc")]
        [InlineData("/api/tags?page=2&sortBy=share&order=asc")]
        [InlineData("/api/tags?page=2&sortBy=share&order=desc")]
        public async Task GetTagsAsync_ShouldReturnSuccessStatusCode(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        [Theory]
        [InlineData("/api/tags?page=1&sortBy=a&order=asc")]
        [InlineData("/api/tags?page=1&sortBy=name&order=a")]
        [InlineData("/api/tags?page=0&sortBy=share&order=asc")]
        [InlineData("/api/tags?page=&sortBy=a&order=asc")]
        [InlineData("/api/tags?page=1&sortBy=&order=a")]
        [InlineData("/api/tags?page=0&sortBy=share&order=")]
        public async Task GetTagsAsync_ShouldReturnBadRequestStatusCode(string url)
        {
            var response = await _httpClient.GetAsync(url);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldReturnExpectedResponse()
        {
            var model = await _httpClient.GetFromJsonAsync<List<TagModelTests>>("/api/tags?page=1&sortBy=name&order=asc");
            Assert.NotNull(model);
            Assert.Equal(50, model.Count);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldSetExpectedCacheControlHeader()
        {
            var response = await _httpClient.GetAsync("/api/tags?page=1&sortBy=name&order=asc");
            var header = response.Headers.CacheControl;
            Assert.NotNull(header);
            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(5), header.MaxAge);
            Assert.True(header.Public);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldReturnExpectedContent()
        {
            var tagsRepositoryMock = new TagsRepositoryMock();
            var tagsApiClientMock = new TagsApiClientMock();
            var client = _webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<ITagsRepository>(tagsRepositoryMock);
                    services.AddSingleton<ITagsApiClient>(tagsApiClientMock);
                });
            }).CreateDefaultClient();

            var model = await client.GetFromJsonAsync<List<TagModelTests>>("/api/tags?page=1&sortBy=name&order=asc");
            Assert.NotNull(model);
            Assert.Equal(4, model.Count);
            Assert.Contains("a", new List<string>() { model[0].Name, model[1].Name, model[2].Name, model[3].Name });
            Assert.Contains("b", new List<string>() { model[0].Name, model[1].Name, model[2].Name, model[3].Name });
            Assert.Contains("c", new List<string>() { model[0].Name, model[1].Name, model[2].Name, model[3].Name });
            Assert.Contains("d", new List<string>() { model[0].Name, model[1].Name, model[2].Name, model[3].Name });

        }
        [Fact]
        public async Task FetchNewTagsAsync_ShouldReturnSuccessStatusCode()
        {
            var response = await _httpClient.PostAsync("/api/tags", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
