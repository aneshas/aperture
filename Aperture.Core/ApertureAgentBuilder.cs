using Aperture.Core.Supervisors;

namespace Aperture.Core
{
    public static class ApertureAgentBuilder
    {
        public static ApertureAgent CreateDefault() =>
            ApertureAgent
                .Instance()
                .UseOneForOneSupervision();
    }
}