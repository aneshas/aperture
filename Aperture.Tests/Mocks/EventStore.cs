using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Core.EventStreams;

namespace Aperture.Tests.Mocks
{
    public class EventStore : IEventStore
    {
        public List<IEvent> Events { get; }

        public EventStore(List<IEvent> events)
        {
            Events = events;
        }
        
        public Task<IEnumerable<EventData>> LoadEventsAsync(Type projection, int fromOffset, int count) =>
            Task.FromResult(
                Events
                    .Skip(fromOffset)
                    .Take(count)
                    .Select((x, i) => new EventData
                    {
                        Offset = i,
                        Event = x
                    })
            );
    }
}