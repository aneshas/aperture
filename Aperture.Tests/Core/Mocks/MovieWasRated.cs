namespace Aperture.Tests.Core.Mocks
{
    public class MovieWasRated : IEvent
    {
        public int Rating { get; }

        public MovieWasRated(int rating)
        {
            Rating = rating;
        }
    }
}