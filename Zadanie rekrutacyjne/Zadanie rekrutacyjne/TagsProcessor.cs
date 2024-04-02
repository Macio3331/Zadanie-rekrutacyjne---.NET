using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne
{
    /// <summary>
    /// Class representing a processor for StackOverflow API's responses.
    /// </summary>
    public class TagsProcessor
    {
        /// <summary>
        /// Method used for acquiring information about tags and storing them inside of TagModel objects.
        /// </summary>
        /// <param name="baseUrl">The rest of the URL of the API.</param>
        /// <param name="noPages">Number of pages to acquire from API.</param>
        /// <returns>List of TagModel objects containing data sent by API.</returns>
        /// <exception cref="HttpRequestException">Exception thrown when there was an issue while retriving the data.</exception>
        public static async Task<List<TagModel>> Load(string baseUrl, int noPages) 
        {
            List<TagModel> list = new List<TagModel>();
            long totalCount = 0;
            for (int i = 1; i <= noPages; i++)
            {
                string url = baseUrl + i.ToString();
                using(HttpResponseMessage response = await StackOverflowApiHelper.apiClient.GetAsync(url))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        TagQuery tagList = await response.Content.ReadAsAsync<TagQuery>();
                        foreach(var tag in tagList.Items)
                        {
                            if (tag.Name == null) continue;
                            totalCount += (long)tag.Count;
                            list.Add(tag);
                        }
                    }
                    else throw new HttpRequestException(response.ReasonPhrase);
                }
            }
            return CalculateShare(list, totalCount);
        }
        /// <summary>
        /// Method used for calculating the share of each tag's number of usages to all tags number of usages.
        /// </summary>
        /// <param name="list">List of TagModel objects containing data sent by API.</param>
        /// <param name="totalCount">Number of all tags' usages in population.</param>
        /// <returns></returns>
        private static List<TagModel> CalculateShare(List<TagModel> list, long totalCount)
        {
            foreach (var tag in list)
                tag.Share = (double)tag.Count / (double)totalCount * 100;
            return list;
        }
    }
}
