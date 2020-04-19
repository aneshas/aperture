namespace Aperture.Tests.Mocks
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