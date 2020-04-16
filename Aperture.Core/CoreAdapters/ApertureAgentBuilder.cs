namespace Aperture.Core.CoreAdapters
{
    public static class ApertureAgentBuilder
    {
        public static ApertureAgent BuildDefault(IEventStore eventStore) =>
            ApertureAgent
                .Instance()
                .UseSupervisionStrategy(new OneForOne())
                .Configure(cfg =>
                {
                    // TODO - Set default configuration
                });
    }
}