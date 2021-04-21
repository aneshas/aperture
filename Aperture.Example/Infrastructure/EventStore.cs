using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Example.Events;
using Aperture.Example.Projections;
using Microsoft.Extensions.Logging;

namespace Aperture.Example.Infrastructure
{
    public class EventStore : IEventStore
    {
        private readonly ILogger<Worker> _logger;

        public EventStore(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<EventData>> LoadEventsAsync(Type projection, int fromOffset, int count)
        {
            List<EventData> eventData;
            
            _logger.LogError($"Loading from {fromOffset}"); 

            if (projection == typeof(SciFiMoviesServerProjection))
            {
                eventData = new List<EventData>
                {
                    new EventData
                    {
                        Offset = 0,
                        Event = new MovieAddedToCatalogue("Matrix", Genre.SciFi)
                    },
                    new EventData
                    {
                        Offset = 1,
                        Event = new MovieAddedToCatalogue("Lost in Space", Genre.SciFi)
                    }
                };
            }
            else
            {
                eventData = new List<EventData>
                {
                    new EventData
                    {
                        Offset = 0,
                        Event = new MovieAddedToCatalogue("Gone in 60s", Genre.Crime)
                    },
                    new EventData
                    {
                        Offset = 1,
                        Event = new MovieAddedToCatalogue("Die Hard", Genre.Crime)
                    }
                };
            }

            return Task.FromResult<IEnumerable<EventData>>(eventData);
        }
    }
}