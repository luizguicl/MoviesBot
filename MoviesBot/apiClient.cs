using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MoviesBot
{
    public class ApiClient
    {
        private string _postReceivedUrl = "/handlers/external/received/";
        private string _postSentUrl = "/handlers/external/sent/";
        private string _postDeliveredUrl = "/handlers/external/delivered/";
        private string _postFailedUrl = "/handlers/external/failed/";
        private string _personUri = "search/person";
        private string _movieUri = "search/movie";
        private string _genreUri = "search/person";
        private string _similarMovies = "movie/id/similar/";



        private static HttpClient _client;
        private AppSettings _appSettings;

        public ApiClient(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_appSettings.MovieDbBaseUrl);
        }

        public async Task<JObject> GetPerson(string name)
        {
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("api_key", _appSettings.ApiKey),
                new KeyValuePair<string, string>("query", name ),
            });

            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, "search/person?", parameters));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JObject.Parse(content));
        }

        public async Task<JObject> GetMovie(string name)
        {
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("api_key", _appSettings.ApiKey),
                new KeyValuePair<string, string>("query", name ),
            });

            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, "search/movie?", parameters));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JObject.Parse(content));
        }
    }
}
