using System.Net;
using System.Net.Http.Headers;

namespace Zadanie_rekrutacyjne
{
    /// <summary>
    /// Class representing a client of StackOverflow API.
    /// </summary>
    public class StackOverflowApiHelper
    {
        /// <summary>
        /// HTTP client handler used for decompressing the response of StackOverflow API.
        /// </summary>
        public static HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
        /// <summary>
        /// The HTTTP client used for getting connections with StackOverflow API.
        /// </summary>
        public static HttpClient apiClient { get; set; } = new HttpClient(handler);
        /// <summary>
        /// Method used for initialization of the client.
        /// </summary>
        /// <param name="baseAddress">Base address of the site.</param>
        public static void InitializeClient(string baseAddress)
        {
            apiClient.BaseAddress = new Uri(baseAddress);
            apiClient.Timeout = TimeSpan.FromSeconds(5);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            apiClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            apiClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }
    }
}
