using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public abstract class Projection : IProjectEvents
    {
        private readonly ITrackOffset _offsetTracker;

        private readonly Type _handlerType = typeof(IHandle<>);

        private List<Type> _types;

        protected Projection()
        {
            _offsetTracker = new NoOpOffsetTracker();
        }

        protected Projection(ITrackOffset offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        public async Task ProjectAsync(IStreamEvents streamEvents, CancellationToken ct)
        {
            var projection = GetType();
            var projectionOffset = await _offsetTracker.GetOffsetAsync(projection);

            try
            {
                await streamEvents.SubscribeAsync(
                    projection,
                    projectionOffset,
                    ct,
                    async data => await TrackAndHandleEventAsync(projection, data));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApertureProjectionException(
                    "Exception encountered while projecting events, see inner exception for more details", e);
            }
        }

        protected virtual async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
        {
            await HandleEventAsync(eventData.Event);
            await _offsetTracker.SaveOffsetAsync(projection, eventData.Offset);
        }

        protected async Task HandleEventAsync(object @event)
        {
            // TODO
            // Call TrackAndHandleEventAsync of each child lookup projection 
            // in ascending order of offsets. - how?
            // Calling from here would ensure that exceptions from lookups are propagated 
            // which means parent projection does not get built if lookups throw ex.

            _types ??= GetType()
                .GetInterfaces()
                .Where(x =>
                    x.IsGenericType
                    && x.GetGenericTypeDefinition() == _handlerType)
                .ToList();

            var handler = _types
                .FirstOrDefault(x =>
                    x.GenericTypeArguments.First() == @event.GetType());

            if (handler == null) return;

            var method = handler.GetMethods().First();

            await (Task) method.Invoke(this, new[] {@event});
        }
    }
}