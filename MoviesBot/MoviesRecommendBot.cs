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

        private readonly String MOVIEDB_BASE_URL = "http://image.tmdb.org/t/p/";
        private readonly String SIZE = "w185";

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
                        Text = Constants.SHOW_TOP_5_OPTION,
                        Value = new PlainText { Text = Constants.SHOW_TOP_5_COMMAND}
                    },

                }
            };

            await _sender.SendMessageAsync(select, owner, cancellationToken);
        }



        public async Task SendTop5MoviesAsync(List<Movie> movieList, Node owner, CancellationToken cancellationToken)
        {
            var select = new Select();
            select.Text = "Escolha uma das opções";

            var options = new List<SelectOption>(); 
               
            foreach(Movie m in movieList)
            {
                options.Add(new SelectOption
                {
                    Text = m.title,
                    Value = new PlainText { Text = m.id.ToString() }
                });
            }

            select.Options = options.ToArray();

            await _sender.SendMessageAsync(select, owner, cancellationToken);
        }

        public async Task SendMovieDataAsync(Movie movie, Node from, CancellationToken cancellationToken)
        {
            var uriLink = new Uri(MOVIEDB_BASE_URL + SIZE + movie.poster_path);
            var uriPreviewLink = new Uri(MOVIEDB_BASE_URL + SIZE + movie.backdrop_path);
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
