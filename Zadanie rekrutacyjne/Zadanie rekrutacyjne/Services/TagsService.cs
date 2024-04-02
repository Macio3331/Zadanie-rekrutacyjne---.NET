using Microsoft.EntityFrameworkCore;
using Serilog;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Services
{
    /// <summary>
    /// Service implementing the logic of the program. It covers methods connected with tags management. 
    /// </summary>
    public class TagsService : ITagsService
    {
        /// <summary>
        /// Object representing a database.
        /// </summary>
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Number of tags returned by service for one page. 
        /// </summary>
        private const int pageSize = 50;
        /// <summary>
        /// Default constructor of the service.
        /// </summary>
        /// <param name="context">Object representing a database.</param>
        public TagsService(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Method used for getting the tags.
        /// </summary>
        /// <param name="page">Number of page to show.</param>
        /// <param name="sortBy">Specifies if data should be sorted by count share in population or by name. Accepted values: "name", "share".</param>
        /// <param name="order">Specifies the order of sorting. Accepts values: "asc" (ascending order), "desc" (descending order).</param>
        /// <returns>List of 50 TagModel objects enclosed in ResultObject (status code and body of response).</returns>
        public async Task<List<TagModel>> GetTags(int page, string sortBy, string order)
        {
            Log.Information("GET service method invoked.");
            if ((sortBy != "name" && sortBy != "share") || (order != "asc" && order != "desc") || (page - 1) * pageSize >= _context.Tags.Count())
                throw new ArgumentException("Bad query parameter.");
            bool lastPage = (page * pageSize > _context.Tags.Count());
            bool ascOrder = (order == "asc");
            bool sortByName = (sortBy == "name");
            List<TagModel> tags = new List<TagModel>();
            if (!lastPage)
            {
                if (ascOrder)
                {
                    if (sortByName) tags = _context.Tags.ToList().OrderBy(t => t.Name).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                    else tags = _context.Tags.ToList().OrderBy(t => t.Share).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                }
                else
                {
                    if (sortByName) tags = _context.Tags.ToList().OrderByDescending(t => t.Name).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                    else tags = _context.Tags.ToList().OrderByDescending(t => t.Share).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                }
            }
            else
            {
                if (ascOrder)
                {
                    if (sortByName) tags = await _context.Tags.OrderBy(t => t.Name).Take(new Range((page - 1) * pageSize, _context.Tags.Count())).ToListAsync();
                    else tags = await _context.Tags.OrderBy(t => t.Share).Take(new Range((page - 1) * pageSize, _context.Tags.Count())).ToListAsync();
                }
                else
                {
                    if (sortByName) tags = await _context.Tags.OrderByDescending(t => t.Name).Take(new Range((page - 1) * pageSize, _context.Tags.Count())).ToListAsync();
                    else tags = await _context.Tags.OrderByDescending(t => t.Share).Take(new Range((page - 1) * pageSize, _context.Tags.Count())).ToListAsync();
                }
            }
            return tags;
        }
        /// <summary>
        /// Method used for resending request to StackOverflow API that makes program upload data into database.
        /// </summary>
        public async Task Seed()
        {
            Log.Information("POST service method invoked.");
            try
            {
                List<TagModel> list = await TagsProcessor.Load("2.3/tags?pagesize=50&order=desc&sort=popular&site=stackoverflow&filter=!bMsg5CXICdlFSp&page=", 25);
                await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Tags");
                await _context.Tags.AddRangeAsync(list);
                _context.SaveChanges();
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
