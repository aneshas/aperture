using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.CoreAdapters
{
    public class PullEventStream : IEventStream
    {
        public TimeSpan PullInterval { get; } = TimeSpan.FromMilliseconds(200);

        public int BatchSize { get; } = 500;

        private readonly IEventStore _eventStore;

        public PullEventStream(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public PullEventStream(IEventStore eventStore, TimeSpan pullInterval, int batchSize)
        {
            _eventStore = eventStore;
            PullInterval = pullInterval;
            BatchSize = batchSize;
        }

        // TODO Maybe add cancellation token 
        public async Task SubscribeAsync(Type projection, int fromOffset, CancellationToken ct, Action<EventData> handleEvent)
        {
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                
                // TODO - Implement queuing here since multiple projections will be calling in
                // + interval - probably call another private func that will sequence these calls
                var eventBatch = await _eventStore.LoadEventsAsync(projection, fromOffset, BatchSize);
                
                if(eventBatch == null) continue;

                foreach (var eventData in eventBatch)
                    handleEvent(eventData);
            }
        }
    }
}