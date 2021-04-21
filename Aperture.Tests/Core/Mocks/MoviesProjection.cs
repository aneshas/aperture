using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;

namespace Aperture.Tests.Core.Mocks
{
    public class MoviesProjection : Projection,
        IHandle<MovieAddedToCatalogue>,
        IHandle<MovieWasRated>
    {
        public List<IEvent> Events { get; } = new List<IEvent>();

        public MoviesProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            Console.WriteLine($"Starting {GetType().Name}...");
        }

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