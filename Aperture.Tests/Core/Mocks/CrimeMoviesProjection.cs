using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Sql;

namespace Aperture.Tests.Core.Mocks
{
    public class CrimeMoviesProjection : SqlProjection,
        IHandle<MovieAddedToCatalogue>
    {
        public List<IEvent> Events { get; } = new List<IEvent>();

        public CrimeMoviesProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            Console.WriteLine($"Starting {GetType().Name}...");
        }

        public Task HandleAsync(MovieAddedToCatalogue @event)
        {
            if (@event.Genre != Genre.Crime) return Task.CompletedTask;
            
            Events.Add(@event);

            return Task.CompletedTask;
        }
    }
}