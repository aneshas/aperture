using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public abstract class ApertureProjection : IProjectEvents
    {
        private readonly ITrackOffset _offsetTracker;

        private readonly Type _handlerType = typeof(IHandleEvent<>);

        protected ApertureProjection(ITrackOffset offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        public async Task ProjectAsync(IStreamEvents streamEvents, CancellationToken ct)
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

        protected async Task HandleEventAsync(object @event)
        {
            var handler = GetType()
                .GetInterfaces()
                .FirstOrDefault(x =>
                    x.IsGenericType
                    && x.GetGenericTypeDefinition() == _handlerType
                    && x.GenericTypeArguments.First() == @event.GetType());

            if (handler == null) return;

            var method = handler.GetMethods().First();

            await (Task) method.Invoke(this, new[] {@event});
        }
    }
}