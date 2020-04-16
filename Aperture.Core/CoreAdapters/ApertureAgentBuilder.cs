namespace Aperture.Core.CoreAdapters
{
    public static class ApertureAgentBuilder
    {
        public static ApertureAgent BuildDefault(IEventStore eventStore) =>
            ApertureAgent
                .Instance()
                .UseEventStream(new PullEventStream(eventStore))
                .UseSupervisionStrategy(new OneForOne());
    }
}