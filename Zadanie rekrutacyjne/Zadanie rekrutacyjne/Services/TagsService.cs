using Microsoft.EntityFrameworkCore;
using Serilog;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Services
{
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext _context;
        private const int pageSize = 50;

        public TagsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TagModel>> GetTags(int page, string sortBy, string order)
        {
            Log.Information("GET service method invoked.");
            if ((sortBy != "name" && sortBy != "share") || (order != "asc" && order != "desc") || (page - 1) * pageSize >= _context.Tags.Count())
            {
                throw new ArgumentException("Bad query parameter.");
            }
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

        public async Task Seed()
        {
            Log.Information("POST service method invoked.");
            try
            {
                List<TagModel> list = await TagsLoader.Load();
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
