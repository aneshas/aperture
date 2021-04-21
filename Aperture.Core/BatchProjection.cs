using System;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public abstract class BatchProjection : Projection
    {
        protected abstract int BatchSize { get; }

        private readonly ITrackOffset _offsetTracker;

        private int _handledEventCount = 0;

        protected BatchProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
        {
            await HandleEventAsync(eventData.Event);
            _handledEventCount++;

            if (_handledEventCount >= BatchSize)
            {
                // TODO - Call Flush
                // This projection can be problematic (Flush would need to be called periodically)
                // eg. call flush after n milliseconds of inactivity, set a flag, throw exceptions if 
                // TrackAndHandleEventAsync is called during flush
                // Also Flush and SaveOffset might be in a transaction (maybe have sql version of this projection also)
                await _offsetTracker.SaveOffsetAsync(projection, eventData.Offset);
                _handledEventCount = 0;
            }
        }
    }
}