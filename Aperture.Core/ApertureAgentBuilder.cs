using Aperture.Core.EventStreams;
using Aperture.Core.SupervisionStrategies;

namespace Aperture.Core
{
    public static class ApertureAgentBuilder
    {
        public static ApertureAgent CreateDefault(IEventStore eventStore) =>
            ApertureAgent
                .Instance()
                .UsePullEventStream(eventStore)
                .UseOneForOneSupervision()
                .Configure(cfg =>
                {
                    // TODO - Set default configuration
                });
    }
}