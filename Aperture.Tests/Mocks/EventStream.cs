using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;

namespace Aperture.Tests.Mocks
{
    public class EventStream : IStreamEvents
    {
        public List<IEvent> Events { get; }

        public EventStream(List<IEvent> events)
        {
            Events = events;
        }

        public Task SubscribeAsync(
            Type projection, int fromOffset,
            CancellationToken ct,
            Func<EventData, Task> handleEvent)
        {
            foreach (var @event in Events)
            {
                handleEvent(new EventData
                {
                    Offset = 0,
                    Event = @event 
                });
            }
            
            return Task.CompletedTask;
        }
    }
}