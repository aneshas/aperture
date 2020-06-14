using Aperture.Core;

namespace Aperture.Polly
{
    public static class ApertureAgentExtensions
    {
        public static ApertureAgent UseRestartWithBackOffSupervision(
            this ApertureAgent agent,
            RestartWithBackOff.Config config = null)
        {
            agent.UseSupervisor(new RestartWithBackOff(config ?? new RestartWithBackOff.Config()));

            return agent;
        }
    }
}