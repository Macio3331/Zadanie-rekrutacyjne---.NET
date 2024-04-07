using Moq;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Services
{
    public class TagsServiceTests
    {
        private readonly TagsService _service;
        private readonly Mock<ITagsApiClient> _tagsApiClientMock;
        private readonly Mock<ITagsRepository> _repositoryMock;
        public TagsServiceTests()
        {
            _tagsApiClientMock = new Mock<ITagsApiClient>();
            
            _repositoryMock = new Mock<ITagsRepository>();
            _repositoryMock.Setup(m => m.GetAllTagsAsync()).ReturnsAsync(
                new List<TagModel>()
                    {
                        new TagModel() { Id = 1, Count = 3, Name = "b", Share = 0.15 },
                        new TagModel() { Id = 2, Count = 2, Name = "c", Share = 0.1 },
                        new TagModel() { Id = 3, Count = 5, Name = "a", Share = 0.25 },
                        new TagModel() { Id = 4, Count = 10, Name = "d", Share = 0.5 }
                    }
            );

            _service = new TagsService(_repositoryMock.Object, _tagsApiClientMock.Object);
        }

        [Fact]
        public async Task GetTagsAsync_ShouldGiveValidTagsSortedByNameAsc()
        {
            var result = await _service.GetTagsAsync(1, "name", "asc", 4);

            Assert.Equal(4, result.Count);
            Assert.Equal("a", result[0].Name);
            Assert.Equal("b", result[1].Name);
            Assert.Equal("c", result[2].Name);
            Assert.Equal("d", result[3].Name);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldGiveValidTagsSortedByNameDesc()
        {
            var result = await _service.GetTagsAsync(1, "name", "desc", 4);

            Assert.Equal(4, result.Count);
            Assert.Equal("a", result[3].Name);
            Assert.Equal("b", result[2].Name);
            Assert.Equal("c", result[1].Name);
            Assert.Equal("d", result[0].Name);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldGiveValidTagsSortedByShareAsc()
        {
            var result = await _service.GetTagsAsync(1, "share", "asc", 4);

            Assert.Equal(4, result.Count);
            Assert.Equal(0.1, result[0].Share);
            Assert.Equal(0.15, result[1].Share);
            Assert.Equal(0.25, result[2].Share);
            Assert.Equal(0.5, result[3].Share);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldGiveValidTwoPagesWithEqualElementsPerPage()
        {
            var result = await _service.GetTagsAsync(1, "share", "desc", 2);

            Assert.Equal(2, result.Count);
            Assert.Equal(0.5, result[0].Share);
            Assert.Equal(0.25, result[1].Share);

            result = await _service.GetTagsAsync(2, "share", "desc", 2);

            Assert.Equal(2, result.Count);
            Assert.Equal(0.15, result[0].Share);
            Assert.Equal(0.1, result[1].Share);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldGiveValidTwoPagesWithoutEqualElementsPerPage()
        {
            var result = await _service.GetTagsAsync(1, "share", "desc", 3);

            Assert.Equal(3, result.Count);
            Assert.Equal(0.5, result[0].Share);
            Assert.Equal(0.25, result[1].Share);
            Assert.Equal(0.15, result[2].Share);

            result = await _service.GetTagsAsync(2, "share", "desc", 3);

            Assert.Single(result);
            Assert.Equal(0.1, result[0].Share);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldThrowInvalidPageException()
        {
            var result = await Assert.ThrowsAsync<ArgumentException>(async() => await _service.GetTagsAsync(2, "share", "desc", 4));
            Assert.Equal("Invalid 'page' query parameter.", result.Message);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldThrowInvalidSortByException()
        {
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetTagsAsync(1, "notShare", "desc", 4));
            Assert.Equal("Invalid 'sortBy' query parameter. It must be 'name' or 'share'.", result.Message);
        }
        [Fact]
        public async Task GetTagsAsync_ShouldThrowInvalidOrderException()
        {
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await _service.GetTagsAsync(1, "share", "notDesc", 4));
            Assert.Equal("Invalid 'share' query parameter. It must be 'asc' or 'desc'.", result.Message);
        }
        [Fact]
        public async Task SeedAsync_ShouldThrowHttpRequestException()
        {
            _tagsApiClientMock.Setup(m => m.LoadAsync(It.IsAny<QueryTagsParameters>()))
                .ThrowsAsync(
                    new HttpRequestException()
            );

            await Assert.ThrowsAsync<HttpRequestException>(async () => await _service.SeedAsync());
        }
        [Fact]
        public async Task SeedASync_ShouldThrowException()
        {
            _tagsApiClientMock.Setup(m => m.LoadAsync(It.IsAny<QueryTagsParameters>()))
                .ThrowsAsync(
                    new Exception()
            );

            await Assert.ThrowsAsync<Exception>(async () => await _service.SeedAsync());
        }
    }
}
