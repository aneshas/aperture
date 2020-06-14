namespace Aperture.Core.Supervisors
{
    public static class ApertureAgentExtensions
    {
        public static ApertureAgent UseOneForOneSupervision(this ApertureAgent agent) =>
            agent.UseSupervisor(new OneForOne());

        public static ApertureAgent UseOneForAllSupervision(this ApertureAgent agent) =>
            agent.UseSupervisor(new OneForAll());
    }
}