using System;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public sealed class NoOpOffsetTracker : ITrackOffset
    {
        public Task SaveOffsetAsync(Type projection, int currentOffset) => Task.CompletedTask;

        public Task<int> GetOffsetAsync(Type projection) => Task.FromResult(0);
    }
}