using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MoviesBot
{
    public class ApiClient
    {
        
        private string _searchPersonUri = "search/person";
        private string _searchMovieUri = "search/movie";
        private string _genreListUri = "genre/movie/list?";
        private string _topRatedMoviesUri = "movie/top_rated";

        private static HttpClient _client;
        private AppSettings _appSettings;

        public ApiClient(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_appSettings.MovieDbBaseUrl);
        }

        public async Task<Person> GetPerson(string name)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", _appSettings.ApiKey);
            parameters.Add("query", name);
            parameters.Add("language", "pt-br");

            var queryString = ToQueryString(parameters);

            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, _searchPersonUri, queryString));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PersonResponse>(content);

            return result.results.First();
        }

        public async Task<Movie> GetMovie(string name)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", _appSettings.ApiKey);
            parameters.Add("query", name);
            parameters.Add("language", "pt-br");

            var queryString = ToQueryString(parameters);


            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, _searchMovieUri, queryString));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MoviesResponse>(content);

            return result.results.First();
        }


        public async Task<List<Movie>> GetSimilarMovies(string movieId)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", _appSettings.ApiKey);
            parameters.Add("language", "pt-br");
            parameters.Add("id", movieId);
            parameters.Add("page", "1");

            var queryString = ToQueryString(parameters);
            
            var similarMovieUri = string.Format("movie/{0}/similar", movieId);
            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, similarMovieUri, queryString));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MoviesResponse>(content);

            return result.results.Take(5).ToList();
        }


        //Retorna o Id do gênero pelo nome
        public async Task<int> GetGenreByName(string name)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", _appSettings.ApiKey);
            parameters.Add("language", "pt-br");


            var queryString = ToQueryString(parameters);
            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, _genreListUri, queryString));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Genre>(content);

            return result.id;
        }


        //Busca 5 filmes mais bem avaliados de um determinado gênero
        public async Task<List<Movie>> GetMoviesByGenreId(string genreId)
        {

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", _appSettings.ApiKey);
            parameters.Add("language", "pt-br");
            parameters.Add("id", genreId);
            parameters.Add("page", "1");

            var queryString = ToQueryString(parameters);
            var genreUri = string.Format("genre/{0}/movies", genreId);

            var response = await _client.GetAsync(string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, genreUri, queryString));

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<MoviesResponse>(content);

            return result.results.Take(5).ToList();
        }

        //Busca 5 filmes mais bem avaliados do IMDB
        public async Task<List<Movie>> GetTopRatedMovies()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", _appSettings.ApiKey);
            parameters.Add("language", "pt-br");
            parameters.Add("page", "1");

            var queryString = ToQueryString(parameters);

            var uri = string.Format("{0}{1}{2}", _appSettings.MovieDbBaseUrl, _topRatedMoviesUri, queryString);

            var response = await _client.GetAsync(uri);

            //will throw an exception if not successful
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MoviesResponse>(content);

            return result.results.Take(5).ToList();
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
