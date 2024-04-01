using System.Net;
using System.Net.Http.Headers;

namespace Zadanie_rekrutacyjne
{
    public class StackOverflowApiHelper
    {
        public static HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
        public static HttpClient apiClient {  get; set; } = new HttpClient(handler);
        public static void InitializeClient()
        {
            apiClient.BaseAddress = new Uri("https://api.stackexchange.com/");
            apiClient.Timeout = TimeSpan.FromSeconds(5);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            apiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }
    }
}
