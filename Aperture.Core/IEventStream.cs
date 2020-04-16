using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IEventStream
    {
        // Subscribe to continuous stream of event data (can we make this a stream?)
        Task SubscribeAsync(Type projection, int fromOffset, CancellationToken ct, Action<EventData> handleEvent);
    }
}