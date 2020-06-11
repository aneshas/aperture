using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IEventStore
    {
        // IEventStore implementation can decide what to do with projection type 
        // eg. query different repo based on the events it handles (eg. via checking for IHandleEvent implementations) 
        // query event store with different params based on the projection type/name  
        // query different topic/stream etc...
        Task<IEnumerable<EventData>> LoadEventsAsync(Type projection, int fromOffset, int count);
    }

    public class PullEventStream : IStreamEvents
    {
        private readonly IEventStore _eventStore;

        // TODO 
        // - configure count, pullInterval?

        public PullEventStream(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task SubscribeAsync(
            Type projection,
            int fromOffset,
            CancellationToken ct,
            Func<EventData, Task> handleEvent)
        {
            var offset = fromOffset;

            // Event store would be queried for each projection separately any way
            // so it doesn't make sense to sequence the calls.
            while (true)
            {
                ct.ThrowIfCancellationRequested();

                await Task.Delay(1000, ct);

                var eventBatch =
                    (await _eventStore.LoadEventsAsync(projection, offset, 100))
                    .ToArray();

                foreach (var eventData in eventBatch)
                    await handleEvent(eventData);

                offset += eventBatch.Count();
            }
        }
    }
}