using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Core.Projections;

namespace Aperture.Tests.Mocks
{
    class SciFiMoviesProjection : TxProjection,
        IHandleEvent<MovieAddedToCatalogue>
    {
        public List<IEvent> Events { get; } = new List<IEvent>();

        public SciFiMoviesProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            Console.WriteLine($"Starting {GetType().Name}...");
        }

        public Task Handle(MovieAddedToCatalogue @event)
        {
            if (@event.Genre != Genre.SciFi) return Task.CompletedTask;
            
            Events.Add(@event);

            return Task.CompletedTask;
        }
    }
}