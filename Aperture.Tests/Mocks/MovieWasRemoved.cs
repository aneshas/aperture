namespace Aperture.Tests.Mocks
{
    public class MovieWasRemoved : IEvent
    {
        public string Reason { get; }

        public MovieWasRemoved(string reason)
        {
            Reason = reason;
        }
    }
}