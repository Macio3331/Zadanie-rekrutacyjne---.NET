using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Repositories
{
    /// <summary>
    /// Repository serving the data from database. It covers methods connected with tags management.
    /// </summary>
    public class TagsRepository : ITagsRepository
    {
        /// <summary>
        /// Object representing a repository.
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Default constructor of the service.
        /// </summary>
        /// <param name="context">Object representing a database.</param>
        public TagsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Method used to get all of the tags stored inside of the database.
        /// </summary>
        /// <returns>List of all tags.</returns>
        public async Task<List<TagModel>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }
        /// <summary>
        /// Method used for truncating the table inside of the database.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public async Task TruncateTableAsync(string tableName)
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE " + tableName);
        }
        /// <summary>
        /// Method used for inserting tags to a database.
        /// </summary>
        /// <param name="tags">List of tags to insert.</param>
        public async Task AddTagsListAsync(List<TagModel> tags)
        {
            await _context.Tags.AddRangeAsync(tags);
            _context.SaveChanges();
        }
    }
}
