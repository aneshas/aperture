using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aperture.Core.EventStreams
{
    public interface IEventStore
    {
        // IEventStore implementation could decide what to do with projection type 
        // eg. query different repo based on the events it handles (TODO add IHandleEvent?) 
        // query event store with different params based on the projection type/name etc... 
        // query different topic/stream 
        // Load event batch
        Task<IEnumerable<EventData>> LoadEventsAsync(Type projection, int fromOffset, int count);
    }
}