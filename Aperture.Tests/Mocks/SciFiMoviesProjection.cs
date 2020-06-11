using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Sql;

namespace Aperture.Tests.Mocks
{
    class SciFiMoviesProjection : SqlProjection,
        IHandleEvent<MovieAddedToCatalogue>
    {
        public List<IEvent> Events { get; } = new List<IEvent>();

        public SciFiMoviesProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            Console.WriteLine($"Starting {GetType().Name}...");
        }

        public Task HandleAsync(MovieAddedToCatalogue @event)
        {
            if (@event.Genre != Genre.SciFi) return Task.CompletedTask;
            
            Events.Add(@event);

            return Task.CompletedTask;
        }
    }
}