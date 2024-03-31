using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne
{
    public class TagsSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public TagsSeeder(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async void Seed()
        {
            try
            {
                List<TagModel> list = await new TagsLoader().Load();
                _context.Tags.ExecuteDelete();
                _context.Tags.AddRange(list);
                _context.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
