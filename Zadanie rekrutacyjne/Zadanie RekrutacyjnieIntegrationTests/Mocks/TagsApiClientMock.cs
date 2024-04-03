using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Mocks
{
    public class TagsApiClientMock : ITagsApiClient
    {
        public Task<List<TagModel>> LoadAsync(QueryTagsParameters queryTagsParameters)
        {
            return Task.FromResult(new List<TagModel>()
            {
                new TagModel() { Id = 1, Count = 3, Name = "b", Share = 0.15 },
                new TagModel() { Id = 2, Count = 2, Name = "c", Share = 0.1 },
                new TagModel() { Id = 3, Count = 5, Name = "a", Share = 0.25 },
                new TagModel() { Id = 4, Count = 10, Name = "d", Share = 0.5 }
            });
        }
    }
}
