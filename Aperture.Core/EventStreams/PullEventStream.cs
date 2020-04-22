using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.EventStreams
{
    public class PullEventStream : IStreamEvents
    {
        private TimeSpan _pullInterval  = TimeSpan.FromMilliseconds(200);

        private readonly int _batchSize  = 500;

        private readonly IEventStore _eventStore;

        public PullEventStream(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public PullEventStream(IEventStore eventStore, TimeSpan pullInterval, int batchSize)
        {
            _eventStore = eventStore;
            
            _pullInterval = pullInterval;
            _batchSize = batchSize;
        }

        public async Task SubscribeAsync(Type projection, int fromOffset, CancellationToken ct, Func<EventData, Task> handleEvent)
        {
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                
                // TODO - Implement queuing here since multiple projections will be calling in
                // + interval - probably call another private func that will sequence these calls
                // it will probably be using a lock with a bounded queue - it should block when full
                var eventBatch = await _eventStore.LoadEventsAsync(projection, fromOffset, _batchSize);
                
                if(eventBatch == null) continue;

                var batchArray = eventBatch.ToArray();
                
                if (batchArray.Length == 0) continue;

                foreach (var eventData in batchArray)
                    await handleEvent(eventData);

                fromOffset += batchArray.Length;
            }
        }
    }
}