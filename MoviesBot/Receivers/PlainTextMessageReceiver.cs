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

            if(!Session.TryGetValue(userIdentity, out currentState))
            {
                Console.WriteLine($"New user: {userIdentity}");
                Session.Add(userIdentity, currentState);
            }

            switch (currentState)
            {
                case State.Start:
                    await ProcessStartState(message, cancellationToken);
                    Session[userIdentity] = State.RecommendMovie;
                    break;
                case State.RecommendMovie:
                    await ProcessRecommendMovieState(message, cancellationToken);
                    break;
                default:
                    //Handle default case
                    break;
            }

            return;
        }

        private async Task ProcessStartState(Message message, CancellationToken cancellationToken)
        {
            await Bot.SendInitialMenuAsync(message.From, cancellationToken);
        }


        private async Task ProcessRecommendMovieState(Message message, CancellationToken cancellationToken)
        {
            await Bot.SendMovieRecommendationAsync(message.From, cancellationToken);
        }

    }
}
