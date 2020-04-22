using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IStreamEvents
    {
        // Subscribe to continuous stream of event data (can we make this a stream?)
        Task SubscribeAsync(Type projection, int fromOffset, CancellationToken ct, Func<EventData, Task> handleEvent);
    }
}