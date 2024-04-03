using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne.Interfaces
{
    /// <summary>
    /// Interface of TagsRepository separating a service and the database.
    /// </summary>
    public interface ITagsRepository
    {
        /// <summary>
        /// Method used to get all of the tags stored inside of the database.
        /// </summary>
        /// <returns>List of all tags.</returns>
        public Task<List<TagModel>> GetAllTagsAsync();
        /// <summary>
        /// Method used for truncating the table inside of the database.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public Task TruncateTableAsync(string tableName);
        /// <summary>
        /// Method used for inserting tags to a database.
        /// </summary>
        /// <param name="tags">List of tags to insert.</param>
        public Task AddTagsListAsync(List<TagModel> tags);
    }
}
