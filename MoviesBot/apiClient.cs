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
        private string _genreUri = "genre/{1}/movies?";
        //private string _similarMoviesUri = "movie/{1}/similar?";
        private string _genreListUri = "genre/movie/list?";



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


        public async Task<JObject> GetSimilarMovies(string movieId)
        {
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("api_key", _appSettings.ApiKey),
                new KeyValuePair<string, string>("id", movieId ),
                new KeyValuePair<string, string>("language", "pt-br" )
            });


            var similarMovieUri = string.Format("movie/{0}/similar?", movieId);
            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, similarMovieUri, parameters));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JObject.Parse(content));
        }


        //Retorna o Id do gênero pelo nome
        public async Task<JObject> GetGenreByName(string name)
        {
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("api_key", _appSettings.ApiKey),
                new KeyValuePair<string, string>("language", "pt-br" ),
            });

            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, _genreListUri, parameters));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JObject.Parse(content));
        }


        //Busca 10 filmes mais bem avaliados de um determinado gênero
        public async Task<JObject> GetMoviesByGenreId(string genreId)
        {
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("api_key", _appSettings.ApiKey),
                new KeyValuePair<string, string>("language", "pt-br" ),
                new KeyValuePair<string, string>("id", genreId )
            });

            var genreUri = string.Format("genre/{0}/movies?", genreId);

            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, genreUri, parameters));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JObject.Parse(content));
        }
    }
}
