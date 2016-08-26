using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesBot
{
    public class TopRatedMoviesResponse
    {
        public int page { get; set; }
        public List<Movie> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
}
