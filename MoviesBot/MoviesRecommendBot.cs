using Lime.Messaging.Contents;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Sender;
using Takenet.MessagingHub.Client.Extensions.Bucket;

namespace MoviesBot
{
    public class MoviesRecommendBot
    {
        private readonly IMessagingHubSender _sender;

        public MoviesRecommendBot(IMessagingHubSender sender)
        {
            _sender = sender;
        }

        public async Task SendInitialMenuAsync(Node owner, CancellationToken cancellationToken)
        {
            var select = new Select
            {
                Text = Constants.INITIAL_MESSAGE,
                Options = new[]
                {
                    new SelectOption
                    {
                        Text = "Opção 1",
                        Value = new PlainText { Text = Constants.RECOMMEND_MOVIE_COMMAND}
                    },

                    new SelectOption
                    {
                        Text = "Opção 2",
                        Value = new PlainText { Text = "Command 2"}
                    },

                     new SelectOption
                    {
                        Text = "Opção 3",
                        Value = new PlainText { Text = "Command 3"}
                    }
                }
            };

            await _sender.SendMessageAsync(select, owner, cancellationToken);
        }

   
        public async Task SendMovieRecommendationAsync(Node from, CancellationToken cancellationToken)
        {
            var uriLink = new Uri("https://i.ytimg.com/vi/KGMqe7_8ORI/maxresdefault.jpg");
            var uriPreviewLink = new Uri("http://thumbs.dreamstime.com/x/funny-vision-test-21065231.jpg");
            var mediaTypeLink = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.JPeg);
            var mediaTypePreviewLink = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.JPeg);

            var mediaLink = new MediaLink
            {
                Uri = uriLink,
                PreviewUri = uriPreviewLink,
                Type = mediaTypeLink,
                PreviewType = mediaTypePreviewLink,
                Size = 1,
                Text = "Image Link"
            };

            await _sender.SendMessageAsync(mediaLink, from, cancellationToken);
        }


    }
}
