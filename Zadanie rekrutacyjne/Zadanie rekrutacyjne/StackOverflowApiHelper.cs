using System.Net;
using System.Net.Http.Headers;

namespace Zadanie_rekrutacyjne
{
    public class StackOverflowApiHelper
    {
        public static HttpClientHandler handler = new HttpClientHandler();
        public static HttpClient apiClient {  get; set; }
        public static void InitializeClient()
        {
            handler.AutomaticDecompression = DecompressionMethods.GZip;
            apiClient = new HttpClient(handler);
            apiClient.BaseAddress = new Uri("https://api.stackexchange.com/");
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            apiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }
    }
}
