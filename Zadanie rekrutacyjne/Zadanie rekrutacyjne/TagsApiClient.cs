using System.Net.Http.Headers;
using System.Net;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne
{
    /// <summary>
    /// Class representing a processor for StackOverflow API's responses.
    /// </summary>
    public class TagsApiClient : ITagsApiClient
    {
        /// <summary>
        /// The HttpClient used for getting connections with StackOverflow API.
        /// </summary>
        public HttpClient _apiClient { get; set; }
        /// <summary>
        /// Default constructor of TagsApiClient. It instatiates HttpClient with proper configuration.
        /// </summary>
        public TagsApiClient(HttpClient httpClient)
        {
            _apiClient = httpClient;
            _apiClient.Timeout = TimeSpan.FromSeconds(5);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            _apiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }
        /// <summary>
        /// Method used for acquiring information about tags and storing them inside of TagModel objects.
        /// </summary>
        /// <param name="queryTagsParameters">Object storing parameters of the query.</param>
        /// <returns>List of TagModel objects containing data sent by API.</returns>
        /// <exception cref="HttpRequestException">Exception thrown when there was an issue while retriving the data.</exception>
        public async Task<List<TagModel>> LoadAsync(QueryTagsParameters queryTagsParameters)
        {
            string urlWithoutPageNumber = queryTagsParameters.baseUrl + "?pagesize=" + queryTagsParameters.pageSize.ToString() + "&order=" + queryTagsParameters.order
                + "&sort=" + queryTagsParameters.sort + "&site=" + queryTagsParameters.site + "&filter=" + queryTagsParameters.filter + "&page=";
            List<TagModel> list = new List<TagModel>();
            long totalCount = 0;
            for (int i = 1; i <= queryTagsParameters.noPages; i++)
            {
                string url = urlWithoutPageNumber + i.ToString();
                HttpResponseMessage response = await _apiClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    TagQuery tagList = await response.Content.ReadAsAsync<TagQuery>();
                    foreach (var tag in tagList.Items)
                    {
                        if (tag.Name == null) continue;
                        totalCount += tag.Count;
                        list.Add(tag);
                    }
                }
                else throw new HttpRequestException(response.ReasonPhrase);
            }
            return CalculateShare(list, totalCount);
        }
        /// <summary>
        /// Method used for calculating the share of each tag's number of usages to all tags number of usages.
        /// </summary>
        /// <param name="list">List of TagModel objects containing data sent by API.</param>
        /// <param name="totalCount">Number of all tags' usages in population.</param>
        /// <returns></returns>
        private List<TagModel> CalculateShare(List<TagModel> list, long totalCount)
        {
            foreach (var tag in list)
                tag.Share = tag.Count / (double)totalCount * 100;
            return list;
        }
    }
}
