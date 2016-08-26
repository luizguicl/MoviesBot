using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesBot
{
    

    public class Person
    {
        public string profile_path { get; set; }
        public bool adult { get; set; }
        public int id { get; set; }
        public List<Movie> known_for { get; set; }
        public string name { get; set; }
        public double popularity { get; set; }
    }
}
