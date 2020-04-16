using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public abstract class ApertureProjection
    {
        private readonly ITrackOffset _offsetTracker;

        protected ApertureProjection(ITrackOffset offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        // This blocks
        public async Task Project(IStreamEvents streamEvents, CancellationToken ct)
        {
            var projection = GetType();
            var projectionOffset = await _offsetTracker.GetOffsetAsync(projection);

            await streamEvents.SubscribeAsync(
                projection, 
                projectionOffset, 
                ct,
                async data => await TrackAndHandleEventAsync(projection, data)); 
        }

        protected abstract Task TrackAndHandleEventAsync(Type projection, EventData eventData);

        protected Task HandleEventAsync(object @event)
        {
            // call handler via reflection
            // but check if projection implements IHandleEvent<@event.Type>
            return Task.CompletedTask;
        }
    }
}