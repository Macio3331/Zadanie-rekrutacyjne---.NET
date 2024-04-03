using Microsoft.OpenApi.Validations;
using Moq;
using Moq.Protected;
using System.Text;
using System.Text.Json;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne
{
    public class TagsApiClientTests
    {
        [Fact]
        public async Task LoadAsync_ShouldReturnCorrectListOfTags()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(
                new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(
                            new TagQuery()
                            {
                                Items = new List<TagModel>
                                {
                                    new TagModel() { Id = 1, Count = 3, Name = "b" },
                                    new TagModel() { Id = 2, Count = 2, Name = "c" },
                                    new TagModel() { Id = 3, Count = 5, Name = "a" },
                                    new TagModel() { Id = 4, Count = 10, Name = "d" }
                                }
                            },
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            }),
                        Encoding.ASCII,
                        "application/json"
                     )
                }
            );

            var tagsApiClient = new TagsApiClient(new HttpClient(handlerMock.Object));
            var result = await tagsApiClient.LoadAsync(new QueryTagsParameters() { baseUrl = "http://localhost:5117", noPages = 1, pageSize = 4 });

            Assert.Equal(4, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(3, result[0].Count);
            Assert.Equal("b", result[0].Name);
            Assert.Equal(15, result[0].Share);
        }
        [Fact]
        public async Task LoadAsync_ShouldThrowHttpRequestException()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(
                new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
            );

            var tagsApiClient = new TagsApiClient(new HttpClient(handlerMock.Object));
            var result = await Assert.ThrowsAsync<HttpRequestException>(async () => await tagsApiClient.LoadAsync(new QueryTagsParameters() { baseUrl = "http://localhost:5117", noPages = 1, pageSize = 4 }));
            Assert.Equal("Service Unavailable", result.Message);
        }
    }
}
