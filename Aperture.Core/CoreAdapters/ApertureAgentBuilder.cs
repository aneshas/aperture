using Aperture.Core.CoreAdapters.EventStreams;
using Aperture.Core.CoreAdapters.SupervisionStrategies;

namespace Aperture.Core.CoreAdapters
{
    public static class ApertureAgentBuilder
    {
        public static ApertureAgent CreateDefault(IEventStore eventStore) =>
            ApertureAgent
                .Instance()
                .UseEventStream(new PullEventStream(eventStore))
                .UseProjectionSupervisor(new OneForOne())
                .Configure(cfg =>
                {
                    // TODO - Set default configuration
                });
    }
}