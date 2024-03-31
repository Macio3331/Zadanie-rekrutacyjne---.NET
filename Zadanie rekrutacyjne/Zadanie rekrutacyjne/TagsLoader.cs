using System.ComponentModel;
using Zadanie_rekrutacyjne.Models;

namespace Zadanie_rekrutacyjne
{
    public class TagsLoader
    {
        const int noPages = 25;
        public async Task<List<TagModel>> Load() 
        {
            List<TagModel> list = new List<TagModel>();
            const string baseUrl = "2.3/tags?pagesize=50&order=desc&sort=popular&site=stackoverflow&filter=!bMsg5CXICdlFSp&page=";
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
                            list.Add(tag);
                        }
                    }
                    else throw new Exception(response.ReasonPhrase);
                }
            }
            return list;
        }
    }
}
