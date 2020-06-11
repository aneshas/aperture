using System;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public class Projection : ApertureProjection
    {
        private readonly ITrackOffset _offsetTracker;

        public Projection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
        {
            await HandleEventAsync(eventData.Event);
            await _offsetTracker.SaveOffsetAsync(projection, eventData.Offset);
        }
    }
}