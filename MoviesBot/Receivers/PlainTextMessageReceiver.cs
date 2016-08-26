using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using System.Collections.Generic;
using System.Linq;

namespace MoviesBot
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        private IDictionary<string, State> Session { get; }
        private IDictionary<string, List<Movie>> Top5Movies { get; }


        private ApiClient _apiClient;
        private readonly IMessagingHubSender _sender;

        protected MoviesRecommendBot Bot { get; }

        public PlainTextMessageReceiver(IMessagingHubSender sender, AppSettings appSettings)
        {
            _sender = sender;
            Session = new Dictionary<string, State>();
            Top5Movies = new Dictionary<string, List<Movie>>();
            Bot = new MoviesRecommendBot(_sender);
            _apiClient = new ApiClient(appSettings);
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            string userIdentity = message.From.Name;

            try
            {
               
                await ProcessMessagesAsync(_sender, message, cancellationToken);

                Console.WriteLine($"From: {message.From} \tContent: {message.Content}");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception processing message: {e}");
                Session[userIdentity] = State.Start;
                await _sender.SendMessageAsync(@"Ops. :(", message.From, cancellationToken);
            }
        }

        private async Task ProcessMessagesAsync(IMessagingHubSender sender, Message message, CancellationToken cancellationToken)
        {
            string userIdentity = message.From.Name;

            State currentState = State.Start;

            if (!Session.TryGetValue(userIdentity, out currentState))
            {
                Console.WriteLine($"New user: {userIdentity}");
                Session.Add(userIdentity, currentState);
            }

            switch (currentState)
            {
                case State.Start:
                    await ProcessStartState(userIdentity, message, cancellationToken);
                    break;
                case State.InitialMenu:
                    await ProcessInitialMenuState(userIdentity, message, cancellationToken);
                    break;
                case State.Top5Movies:
                    await ProcessTop5State(userIdentity, message, cancellationToken);
                    break;
                case State.Top5MoviesOption:
                    await ProcessTop5OptionState(userIdentity, message, cancellationToken);
                    break;
                default:
                    await _sender.SendMessageAsync(Constants.ERROR_MESSAGE, message.From, cancellationToken);
                    Session[userIdentity] = State.Start;
                    break;
            }

            return;
        }

        private async Task ProcessTop5OptionState(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            var option = message.Content.ToString().ToLower();

            if (option.ToLower().Equals(Constants.YES_OPTION))
            {
                await _sender.SendMessageAsync(Constants.FINAL_MESSAGE, message.From, cancellationToken);
                Session[userIdentity] = State.Start;
            }
            else if(option.ToLower().Equals(Constants.NO_OPTION))
            {
                await _sender.SendMessageAsync("Em breve", message.From, cancellationToken);                
            }else
            {
                Session[userIdentity] = State.Start;
            }                       

        }

        private async Task ProcessTop5State(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            var option = message.Content.ToString().ToLower();


            List<Movie> userMovieList;

            Top5Movies.TryGetValue(userIdentity, out userMovieList);

            var resultList = userMovieList.Where(m => m.id.ToString() == option);
            var chosenMovie = resultList.First();

            await Bot.SendMovieDataAsync(chosenMovie, message.From, cancellationToken);


            await Bot.SendHaveYouFindMovie(message.From, cancellationToken);         

            Session[userIdentity] = State.Top5MoviesOption;
        }

        private async Task ProcessInitialMenuState(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            var command = message.Content.ToString().ToLower();

            if (command.Equals(Constants.SHOW_TOP_5_COMMAND.ToLower()))
            {
                await ProcessTop5Command(userIdentity, message.From, cancellationToken);
                Session[userIdentity] = State.Top5Movies;
                return;
            }
            

            Session[userIdentity] = State.InitialMenu;

        }


        private async Task ProcessTop5Command(string userIdentity, Node from, CancellationToken cancellationToken)
        {
            var movieList = await _apiClient.GetTopRatedMovies();
        
            Top5Movies.Add(userIdentity, movieList);
            
            await Bot.SendTop5MoviesAsync(movieList, from, cancellationToken);
        }

        private async Task ProcessStartState(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            await Bot.SendInitialMenuAsync(message.From, cancellationToken);
            Session[userIdentity] = State.InitialMenu;
        }

    }
}
