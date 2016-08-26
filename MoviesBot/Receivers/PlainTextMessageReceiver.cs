using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using System.Collections.Generic;

namespace MoviesBot
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        private IDictionary<string, State> Session { get; }

        private readonly IMessagingHubSender _sender;

        protected MoviesRecommendBot Bot { get; }

        public PlainTextMessageReceiver(IMessagingHubSender sender)
        {
            _sender = sender;
            Session = new Dictionary<string, State>();
            Bot = new MoviesRecommendBot(_sender);
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                await ProcessMessagesAsync(_sender, message, cancellationToken);

                Console.WriteLine($"From: {message.From} \tContent: {message.Content}");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception processing message: {e}");
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
                    break;
            }

            return;
        }

        private Task ProcessTop5OptionState(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            var option = message.Content.ToString().ToLower();

            if (option.Equals("sim"))
            {
                _sender.SendMessageAsync("Em desenvolvimento", message.From, cancellationToken);
            }
            else
            {
                _sender.SendMessageAsync(Constants.FINAL_MESSAGE, message.From, cancellationToken);
            }
            throw new NotImplementedException();
        }

        private async Task ProcessTop5State(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            var option = message.Content.ToString().ToLower();

            //process chosen option

            //Send movie data to user

            await _sender.SendMessageAsync(Constants.MOVIE_CHOSEN, message.From, cancellationToken);

            Session[userIdentity] = State.Top5MoviesOption;

            throw new NotImplementedException();
        }

        private async Task ProcessInitialMenuState(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            var command = message.Content.ToString().ToLower();

            if (command.Equals(Constants.SHOW_TOP_5_COMMAND.ToLower()))
            {
                await ProcessTop5Command(userIdentity, cancellationToken);
                Session[userIdentity] = State.Top5Movies;

            }
            else if (command.Equals(Constants.RECEIVE_SUGGESTION_COMMAND.ToLower()))
            {

            }
            else if (command.Equals(Constants.CHOOSE_GENRE_COMMAND.ToLower()))
            {

            }

            Session[userIdentity] = State.InitialMenu;

        }

        private Task ProcessTop5Command(string userIdentity, CancellationToken cancellationToken)
        {

            //TODO: Get top rated movies and send list to user



            throw new NotImplementedException();
        }

        private async Task ProcessStartState(string userIdentity, Message message, CancellationToken cancellationToken)
        {
            await Bot.SendInitialMenuAsync(message.From, cancellationToken);
            Session[userIdentity] = State.InitialMenu;
        }


        private async Task ProcessRecommendMovieState(Message message, CancellationToken cancellationToken)
        {
            //TODO: 
            await Bot.SendMovieRecommendationAsync(message.From, cancellationToken);
        }

    }
}
