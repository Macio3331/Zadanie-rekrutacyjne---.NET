using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;

namespace Zadanie_rekrutacyjne.Services
{
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext _context;
        public TagsService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
