namespace Aperture.Core.EventStreams
{
    public static class ApertureAgentExtensions
    {

        public static ApertureAgent UsePullEventStream(this ApertureAgent agent, IEventStore eventStore)
        {
            agent.UseEventStream(new PullEventStream(eventStore));

            return agent;
        }
    }
}