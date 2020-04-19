namespace Aperture.Tests.Mocks
{
    public class MovieAddedToCatalogue : IEvent
    {
        public string Title { get; }

        public Genre Genre { get; }

        public MovieAddedToCatalogue(string title, Genre genre)
        {
            Title = title;
            Genre = genre;
        }
    }
}