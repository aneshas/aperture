using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.Supervisors
{
    public class OneForAll : ISuperviseProjection
    {
        public virtual async Task Run(
            IStreamEvents streamEvents,
            IProjectEvents projection,
            Action<Exception> handleException,
            CancellationToken ct) =>
            await projection.ProjectAsync(streamEvents, ct);
    }
}