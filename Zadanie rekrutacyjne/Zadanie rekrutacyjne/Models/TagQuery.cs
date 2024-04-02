namespace Zadanie_rekrutacyjne.Models
{
    /// <summary>
    /// Class used for mapping the content of API's response from JSON to objects.
    /// </summary>
    public class TagQuery
    {
        /// <summary>
        /// List of tags (TagModel objects).
        /// </summary>
        public List<TagModel> Items { get; set; }
    }
}
