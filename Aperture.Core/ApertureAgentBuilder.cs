using Aperture.Core.SupervisionStrategies;

namespace Aperture.Core
{
    public static class ApertureAgentBuilder
    {
        // TODO - Do we need to pull this out somewhere and add quartz pull event stream as default
        public static ApertureAgent CreateDefault() =>
            ApertureAgent
                .Instance()
                .UseRestartWithBackOffSupervision()
                .Configure(cfg =>
                {
                    // TODO - Set default configuration
                });
    }
}