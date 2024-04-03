using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Mocks
{
    public class TagsRepositoryMock : ITagsRepository
    {
        public List<TagModel> Tags { get; set; } = new List<TagModel>();
        public Task AddTagsListAsync(List<TagModel> tags)
        {
            Tags.AddRange(tags);
            return Task.CompletedTask;
        }

        public Task<List<TagModel>> GetAllTagsAsync()
        {
            return Task.FromResult(Tags);
        }

        public Task TruncateTableAsync(string tableName)
        {
            Tags.Clear();
            return Task.CompletedTask;
        }
    }
}
