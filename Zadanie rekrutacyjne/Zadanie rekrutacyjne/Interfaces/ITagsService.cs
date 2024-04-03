using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Interfaces
{
    /// <summary>
    /// Interface of TagsService separating a controller and the service.
    /// </summary>
    public interface ITagsService
    {
        /// <summary>
        /// Method used for getting the tags.
        /// </summary>
        /// <param name="page">Number of page to show.</param>
        /// <param name="sortBy">Specifies if data should be sorted by count share in population or by name. Accepted values: "name", "share".</param>
        /// <param name="order">Specifies the order of sorting. Accepts values: "asc" (ascending order), "desc" (descending order).</param>
        /// <param name="pageSize">Specifies the number of tags on each page. Default value is 50</param>
        /// <returns>List of 50 TagModel objects enclosed in ResultObject (status code and body of response).</returns>
        public Task<List<TagModel>> GetTagsAsync(int page, string sortBy, string order, int pageSize = 50);
        /// <summary>
        /// Method used for resending request to StackOverflow API that makes program upload data into database.
        /// </summary>
        public Task SeedAsync();
    }
}
