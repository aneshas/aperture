using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public abstract class ApertureProjection
    {
        private readonly IOffsetTracker _offsetTracker;

        protected ApertureProjection(IOffsetTracker offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        // This blocks
        public async Task Project(IEventStream eventStream, CancellationToken ct)
        {
            var projection = GetType();
            var projectionOffset = await _offsetTracker.GetOffsetAsync(projection);

            await eventStream.SubscribeAsync(
                projection, 
                projectionOffset, 
                ct,
                async data => await HandleEventAsync(projection, data)); 
        }

        protected abstract Task HandleEventAsync(Type projection, EventData eventData);

        protected Task HandleEventAsync(object @event)
        {
            // call handler via reflection
            // but check if projection implements IHandleEvent<@event.Type>
            return Task.CompletedTask;
        }
    }
}