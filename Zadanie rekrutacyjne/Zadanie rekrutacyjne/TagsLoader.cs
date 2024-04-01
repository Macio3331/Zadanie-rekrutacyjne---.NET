using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne
{
    public static class TagsLoader
    {
        private const int noPages = 25;
        private const int pageSize = 50;
        public static async Task<List<TagModel>> Load() 
        {
            List<TagModel> list = new List<TagModel>();
            string baseUrl = "2.3/tags?pagesize=" + pageSize.ToString() + "&order=desc&sort=popular&site=stackoverflow&filter=!bMsg5CXICdlFSp&page=";
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
                            if (tag.Count == null || tag.Name == null) continue;
                            totalCount += (long)tag.Count;
                            list.Add(tag);
                        }
                    }
                    else throw new HttpRequestException(response.ReasonPhrase);
                }
            }
            foreach(var tag in list)
            {
                tag.Share = (double)tag.Count / (double)totalCount * 100;
            }
            return list;
        }
    }
}
