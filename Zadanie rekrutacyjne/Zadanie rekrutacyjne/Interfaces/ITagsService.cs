using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Interfaces
{
    public interface ITagsService
    {
        public Task<List<TagModel>> GetTags(int page, string sortBy, string order);
        public Task Seed();
    }
}
