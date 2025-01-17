using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace SCM.SwissArmyKnife.Extensions
{
    /// <summary>
    /// Extension methods to simplify common HttpClientExtension such as working with JSON.
    /// </summary>
    public static class HttpClientExtensions
    {

        /// <summary>
        /// GETs the given address, and serializes the resulting JSON object into an object of type T.
        /// Throws an error on non-2xx status messages.
        /// </summary>
        /// <param name="httpClient">HttpClient to use.</param>
        /// <param name="url">Url to retrieve.</param>
        /// <param name="maxCharactersToPrint">
        /// If any errors occurs, how many characters of the response body should be included in the response body?
        /// If set to null, records the entire response.
        /// </param>
        /// <typeparam name="TResponse">The type to attempt to serialize the JSON response to.</typeparam>
        /// <returns>The response from the server as <typeparamref name="TResponse"/>.</returns>
        public static Task<TResponse> GetAsJsonAsync<TResponse>(this HttpClient httpClient, string url, int? maxCharactersToPrint = null)
        {
            return GetAsJsonAsync<TResponse>(httpClient, new Uri(url), maxCharactersToPrint);
        }

        /// <summary>
        /// GETs the given address, and serializes the resulting JSON object into an object of type T.
        /// Throws an error on non-2xx status messages.
        /// </summary>
        /// <param name="httpClient">HttpClient to use.</param>
        /// <param name="url">Url to retrieve.</param>
        /// <param name="maxCharactersToPrint">
        /// If any errors occurs, how many characters of the response body should be included in the response body?
        /// If set to null, records the entire response.
        /// </param>
        /// <typeparam name="TResponse">The type to attempt to serialize the JSON response to.</typeparam>
        /// <returns>The response from the server as <typeparamref name="TResponse"/>.</returns>
        public static async Task<TResponse> GetAsJsonAsync<TResponse>(this HttpClient httpClient, Uri url, int? maxCharactersToPrint = null)
        {
            string? body = null;
            try
            {
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<TResponse>(body);
            }
            catch (HttpRequestException e)
            {
                string? potentiallyTruncatedBody =
                    maxCharactersToPrint.HasValue ? body?.Truncate(maxCharactersToPrint.Value) : body;

                throw new HttpRequestException(
                    $"Exception while trying to request '{url}'. Original message: '{e.Message}'. Got response (potentially truncated) '{potentiallyTruncatedBody}'",
                    e);
            }
            // Something went wrong in the serialization process
            catch (JsonException e)
            {
                string? potentiallyTruncatedBody =
                    maxCharactersToPrint.HasValue ? body?.Truncate(maxCharactersToPrint.Value) : body;

                throw new JsonException(
                    $"Exception while trying deserialize for '{url}'. Original message: '{e.Message}'. Got response (potentially truncated) {potentiallyTruncatedBody}", e
                );
            }
        }



        /// <summary>
        /// POSTS to the <paramref name="url"/>, with a potentially provided <paramref name="jsonBody"/>.
        /// The response is expected to be a JSON body and is serialized to <typeparamref name="TResponse"/>.
        /// </summary>
        /// <param name="httpClient">The HttpClient to use for the operation.</param>
        /// <param name="url">The URL to POST to.</param>
        /// <param name="jsonBody">The object to be serialized to JSON and included in the body. If not specified or null, no body will be sent.</param>
        /// <param name="maxCharactersToPrint">
        /// If an error happen, how many characters of the HTTP response to include in the exception message.
        /// If not provided, will record the entire response.
        /// </param>
        /// <exception cref="HttpRequestException">Will throw HttpRequestException if server responds with non-2xx status code.</exception>
        /// <exception cref="JsonException">Will throw JsonException if it was unable to parse the response body to <typeparamref name="TResponse"/>.</exception>
        /// <typeparam name="TResponse">The response type to parse the returned JSON to.</typeparam>
        /// <returns>The response type serialized into <typeparamref name="TResponse"/>.</returns>
        ///
        public static Task<TResponse> PostAsJsonAsync<TResponse>(this HttpClient httpClient, string url, object? jsonBody = null, int? maxCharactersToPrint = null)
        {
            return PostAsJsonAsync<TResponse>(httpClient, new Uri(url), jsonBody, maxCharactersToPrint);
        }

        /// <summary>
        /// POSTs the given address with a potential provided body. If the body is provided, it is json-serialized.
        /// The JSON response is serialized into an T.
        /// Throws an error on non-2xx status messages.
        /// </summary>
        public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, Uri url, object? jsonBody = null, int? maxCharactersToPrint = null)
        {
            string? body = null;

            try
            {
                HttpContent? content = null;

                if (jsonBody != null)
                {
                    content = new StringContent(JsonConvert.SerializeObject(jsonBody), Encoding.UTF8, "application/json");
                }
                else
                {
                    content = new StringContent(string.Empty);
                }

                var response = await httpClient.PostAsync(url, content).ConfigureAwait(false);
                body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(body);
            }

            catch (HttpRequestException e)
            {
                string? potentiallyTruncatedBody =
                    maxCharactersToPrint.HasValue ? body?.Truncate(maxCharactersToPrint.Value) : body;

                throw new HttpRequestException(
                    $"Exception while trying to post '{url}'. Original message: '{e.Message}'. Got response (potentially truncated) '{potentiallyTruncatedBody}'",
                    e);
            }
            // Something went wrong in the serialization process
            catch (JsonException e)
            {
                string? potentiallyTruncatedBody =
                    maxCharactersToPrint.HasValue ? body?.Truncate(maxCharactersToPrint.Value) : body;

                throw new JsonException(
                    $"Exception while trying deserialize post for '{url}'. Original message: '{e.Message}'. Got response (potentially truncated) {potentiallyTruncatedBody}", e
                );
            }
        }
    }
}
