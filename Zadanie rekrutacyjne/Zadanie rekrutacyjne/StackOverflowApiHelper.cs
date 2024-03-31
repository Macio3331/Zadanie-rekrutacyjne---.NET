using System.Net.Http.Headers;

namespace Zadanie_rekrutacyjne
{
    public class StackOverflowApiHelper
    {
        public static HttpClient apiClient {  get; set; } = new HttpClient();
        public static void InitializeClient()
        {
            apiClient.BaseAddress = new Uri("https://api.stackexchange.com/");
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
