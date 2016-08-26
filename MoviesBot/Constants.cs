using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesBot
{
    public static class Constants
    {
        //Phraseology
        public const string INITIAL_MESSAGE = "Oi! Sou o BotFlicks e vou te ajudar a escolher um filme bacana hoje. Você pode: ";

        public const string ERROR_MESSAGE = "Você pode refazer a sua busca? Quero te ajudar, mas não entendi o que você precisa!";

        public const string MOVIE_CHOSEN = "Você já encontrou o seu filme? Se SIM, maravilha! Se NÃO, envie o gênero do filme.";

        public const string FINAL_MESSAGE = "Hora de pegar a pipoca. Aproveite o filme e depois volte para conferir as novidades. :(";


        //Options
        public const string SHOW_TOP_5_OPTION = "Ver TOP 5";
        public const string RECEIVE_SUGGESTION_OPTION = "Receber sugestão";
        public const string CHOOSE_GENRE_OPTION = "Escolher gênero";

        //Commands
        public const string SHOW_TOP_5_COMMAND = "/showTop5";
        public const string RECEIVE_SUGGESTION_COMMAND = "/receiveSuggestions";
        public const string CHOOSE_GENRE_COMMAND = "/chooseGenre";


    }
}
