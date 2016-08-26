using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesBot
{
    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }

        public enum GenreNumber
        {
            Action = 0,
            Adventure = 1,
            Comedy = 2,
            Drama = 3,
            Horror = 4,
            Romance = 5
        }

        public enum ApiGenreCodes
        {
            Action = 28,
            Adventure = 12,
            Comedy = 35,
            Drama = 18,
            Horror = 27,
            Romance = 10749
        }

        private static ApiGenreCodes ParseGenre(GenreNumber genre)
        {
            switch (genre)
            {
                case GenreNumber.Action:
                    return ApiGenreCodes.Action;
                case GenreNumber.Adventure:
                    return ApiGenreCodes.Adventure;
                case GenreNumber.Comedy:
                    return ApiGenreCodes.Comedy;
                case GenreNumber.Drama:
                    return ApiGenreCodes.Drama;
                case GenreNumber.Horror:
                    return ApiGenreCodes.Horror;
                case GenreNumber.Romance:
                    return ApiGenreCodes.Romance;
            }

            return ApiGenreCodes.Action;
        }
    }
}
