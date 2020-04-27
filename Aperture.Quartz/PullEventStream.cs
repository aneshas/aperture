using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;

namespace Aperture.Quartz
{
    // TODO Register this as a singleton
    public class PullEventStream : IStreamEvents, ISubscriptionBroker
    {
        private const int BatchSize = 100; // TODO Configurable
        
        private readonly ConcurrentDictionary<Type, Subscription> _subscriptions
            = new ConcurrentDictionary<Type, Subscription>();

        public async Task SubscribeAsync(
            Type projection,
            int fromOffset,
            CancellationToken ct,
            Func<EventData, Task> handleEvent)
        {
            var subscription = SubscribeProjection(projection);

            subscription.EnqueueRequest(new EventBatchRequest(projection, fromOffset, BatchSize));

            // Will this cause issues and block thread build?
            try
            {
                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    var response = subscription.DequeueResponse();
                    
                    if(response == null) continue;

                    foreach (var eventData in response.EventBatch)
                        await handleEvent(eventData);

                    fromOffset += response.EventBatch.Count();

                    subscription.EnqueueRequest(new EventBatchRequest(projection, fromOffset, BatchSize));
                }
            }
            catch (Exception)
            {
                UnsubscribeProjection(projection);
                throw;
            }
        }

        private Subscription SubscribeProjection(Type projection)
        {
            var subscription = new Subscription();

            _subscriptions.TryAdd(projection, subscription);

            return subscription;
        }

        private void UnsubscribeProjection(Type projection) =>
            _subscriptions.TryRemove(projection, out _);

        public IEnumerable<Subscription> Subscriptions() =>
            _subscriptions.Select(entry => entry.Value);
    }
}