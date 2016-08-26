using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesBot
{
    public class MovieSearch
    {
        private AppSettings _appSettings;
        private ApiClient _apiClient;

        public MovieSearch(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _apiClient = new ApiClient(_appSettings);
        }

        public void GetPerson(string name)
        {
            _apiClient.GetPerson("kevin bacon");

        }
    }
}
