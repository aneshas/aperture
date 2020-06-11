using Aperture.Core.SupervisionStrategies;

namespace Aperture.Core
{
    public static class ApertureAgentBuilder
    {
        public static ApertureAgent CreateDefault() =>
            ApertureAgent
                .Instance()
                .UseRestartWithBackOffSupervision();
    }
}