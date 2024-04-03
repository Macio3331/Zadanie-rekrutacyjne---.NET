using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Interfaces
{
    /// <summary>
    /// Interface of TagsProcessor separating a service and the processor.
    /// </summary>
    public interface ITagsApiClient
    {
        /// <summary>
        /// Method used for acquiring information about tags and storing them inside of TagModel objects.
        /// </summary>
        /// <param name="queryTagsParameters">Object storing parameters of the query.</param>
        /// <returns>List of TagModel objects containing data sent by API.</returns>
        public Task<List<TagModel>> LoadAsync(QueryTagsParameters queryTagsParameters);
    }
}
