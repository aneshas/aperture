using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;

namespace Aperture.Tests.Core.Mocks
{
    public class MoviesApertureProjection : ApertureProjection,
        IHandle<MovieAddedToCatalogue>,
        IHandle<MovieWasRated>
    {
        public List<IEvent> Events { get; } = new List<IEvent>();

        public MoviesApertureProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            Console.WriteLine($"Starting {GetType().Name}...");
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData) =>
            await HandleEventAsync(eventData.Event);

        public Task HandleAsync(MovieAddedToCatalogue @event)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }

        public Task HandleAsync(MovieWasRated @event)
        {
             Events.Add(@event);
             return Task.CompletedTask;
        }
    }
}