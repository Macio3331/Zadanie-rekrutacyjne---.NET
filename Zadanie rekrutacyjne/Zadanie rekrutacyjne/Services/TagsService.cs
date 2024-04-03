using Microsoft.EntityFrameworkCore;
using Serilog;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;
using Zadanie_rekrutacyjne.Repositories;

namespace Zadanie_rekrutacyjne.Services
{
    /// <summary>
    /// Service implementing the logic of the program. It covers methods connected with tags management. 
    /// </summary>
    public class TagsService : ITagsService
    {
        /// <summary>
        /// Object representing a repository.
        /// </summary>
        private readonly ITagsRepository _repository;
        /// <summary>
        /// Object representing a tags processor.
        /// </summary>
        private readonly ITagsApiClient _tagsApiClient;
        /// <summary>
        /// Default constructor of the service.
        /// </summary>
        /// <param name="repository">Object representing a repository.</param>
        /// <param name="tagsApiClient">Object representing a tags processor.</param>
        public TagsService(ITagsRepository repository, ITagsApiClient tagsApiClient)
        {
            _repository = repository;
            _tagsApiClient = tagsApiClient;
        }
        /// <summary>
        /// Method used for getting the tags.
        /// </summary>
        /// <param name="page">Number of page to show.</param>
        /// <param name="sortBy">Specifies if data should be sorted by count share in population or by name. Accepted values: "name", "share".</param>
        /// <param name="order">Specifies the order of sorting. Accepts values: "asc" (ascending order), "desc" (descending order).</param>
        /// <param name="pageSize">Specifies the number of tags on each page. Default value is 50.</param>
        /// <returns>List of 50 TagModel objects enclosed in ResultObject (status code and body of response).</returns>
        public async Task<List<TagModel>> GetTagsAsync(int page, string sortBy, string order, int pageSize = 50)
        {
            Log.Information("GET service method invoked.");
            if (sortBy != "name" && sortBy != "share")
                throw new ArgumentException("Invalid 'sortBy' query parameter. It must be 'name' or 'share'.");
            if (order != "asc" && order != "desc")
                throw new ArgumentException("Invalid 'share' query parameter. It must be 'asc' or 'desc'.");
            if ((page - 1) * pageSize >= (await _repository.GetAllTagsAsync()).Count())
                throw new ArgumentException("Invalid 'page' query parameter.");
            bool lastPage = (page * pageSize > (await _repository.GetAllTagsAsync()).Count());
            bool ascOrder = (order == "asc");
            bool sortByName = (sortBy == "name");
            List<TagModel> tags = new List<TagModel>();
            if (!lastPage)
            {
                if (ascOrder)
                {
                    if (sortByName) tags = (await _repository.GetAllTagsAsync()).OrderBy(t => t.Name).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                    else tags = (await _repository.GetAllTagsAsync()).OrderBy(t => t.Share).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                }
                else
                {
                    if (sortByName) tags = (await _repository.GetAllTagsAsync()).OrderByDescending(t => t.Name).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                    else tags = (await _repository.GetAllTagsAsync()).OrderByDescending(t => t.Share).Take(new Range((page - 1) * pageSize, page * pageSize)).ToList();
                }
            }
            else
            {
                if (ascOrder)
                {
                    if (sortByName) tags = (await _repository.GetAllTagsAsync()).OrderBy(t => t.Name).Take(new Range((page - 1) * pageSize, (await _repository.GetAllTagsAsync()).Count())).ToList();
                    else tags = (await _repository.GetAllTagsAsync()).OrderBy(t => t.Share).Take(new Range((page - 1) * pageSize, (await _repository.GetAllTagsAsync()).Count())).ToList();
                }
                else
                {
                    if (sortByName) tags = (await _repository.GetAllTagsAsync()).OrderByDescending(t => t.Name).Take(new Range((page - 1) * pageSize, (await _repository.GetAllTagsAsync()).Count())).ToList();
                    else tags = (await _repository.GetAllTagsAsync()).OrderByDescending(t => t.Share).Take(new Range((page - 1) * pageSize, (await _repository.GetAllTagsAsync()).Count())).ToList();
                }
            }
            return tags;
        }
        /// <summary>
        /// Method used for resending request to StackOverflow API that makes program upload data into database.
        /// </summary>
        public async Task SeedAsync()
        {
            Log.Information("POST service method invoked.");
            try
            {

                List<TagModel> list = await _tagsApiClient.LoadAsync(new QueryTagsParameters() { baseUrl = "https://api.stackexchange.com/2.3/tags" });
                await _repository.TruncateTableAsync("Tags");
                await _repository.AddTagsListAsync(list);
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
