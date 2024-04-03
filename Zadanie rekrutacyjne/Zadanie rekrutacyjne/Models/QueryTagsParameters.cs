namespace Zadanie_rekrutacyjne.Models
{
    /// <summary>
    /// Model storing information about query parameters.
    /// </summary>
    public class QueryTagsParameters
    {
        /// <summary>
        /// The base of the URL of the API.
        /// </summary>
        public string baseUrl { get; set; }
        /// <summary>
        /// Number of pages to acquire from API. Default value is 25.
        /// </summary>
        public int noPages { get; set; } = 25;
        /// <summary>
        /// Size of each page. Default value is 50.
        /// </summary>
        public int pageSize { get; set; } = 50;
        /// <summary>
        /// String representing filter type. Default value is '!bMsg5CXICdlFSp'.
        /// </summary>
        public string filter { get; set; } = "!bMsg5CXICdlFSp";
        /// <summary>
        /// Name of the site. Default value is 'stackoverflow'.
        /// </summary>
        public string site { get; set; } = "stackoverflow";
        /// <summary>
        /// The order of sorting. Default value is 'desc'.
        /// </summary>
        public string order { get; set; } = "desc";
        /// <summary>
        /// The value by which tags are sorted. Default value is 'popular'.
        /// </summary>
        public string sort { get; set; } = "popular";
    }
}
