using System.Threading.Tasks;
using Quartz;

namespace Aperture.Quartz
{
    public class LoadEventsJob : IJob
    {
        private readonly IEventStore _eventStore;

        private readonly ISubscriptionBroker _broker; // needs to be singleton

        public LoadEventsJob(IEventStore eventStore, ISubscriptionBroker broker)
        {
            _eventStore = eventStore;
            _broker = broker;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // This will load events for all projections in sequence for this batch
            // could we parallelize this? - probably yes
            foreach (var subscription in _broker.Subscriptions())
            {
                var request = subscription.NextRequest();

                if (request == null) continue;
                
                // Call _eventStore.LoadEvents
                
                subscription.EnqueueResponse(null);
            }
            
            return Task.CompletedTask;
        }
    }
}