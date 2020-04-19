namespace Aperture.Core.SupervisionStrategies
{
    public static class ApertureAgentExtensions
    {
        public static ApertureAgent UseRestartWithBackOffSupervision(this ApertureAgent agent)
        {
            // provide default params
            agent.UseSupervisor(new RestartWithBackOff());

            return agent;
        }

        // TODO - Add one with configurable params

        public static ApertureAgent UseOneForOneSupervision(this ApertureAgent agent)
        {
            agent.UseSupervisor(new OneForOne());

            return agent;
        }

        public static ApertureAgent UseOneForAllSupervision(this ApertureAgent agent)
        {
            agent.UseSupervisor(new OneForAll());

            return agent;
        }
    }
}