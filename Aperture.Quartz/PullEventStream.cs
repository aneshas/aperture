using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;

namespace Aperture.Quartz
{
    // TODO Register this as a singleton
    public class PullEventStream : IStreamEvents, ISubscriptionBroker
    {
        public async Task SubscribeAsync(
            Type projection, 
            int fromOffset, 
            CancellationToken ct,
            Func<EventData, Task> handleEvent)
        {
            // TODO Create a subscription for projection (concurrent dict)

            // Enqueue request via subs

            // Will this cause issues and block thread build?
            try
            {
                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    // TODO Try dequeue resp from subs 
                    // If no work continue
                    var eventBatch = new List<EventData>();

                    foreach (var eventData in eventBatch)
                        await handleEvent(eventData);

                    // Enqueue req updated offset
                }
            }
            catch (Exception)
            {
                // TODO Unsubscribe
                throw;
            }
        }

        public IEnumerable<Subscription> Subscriptions()
        {
            throw new NotImplementedException();
        }
    }
}