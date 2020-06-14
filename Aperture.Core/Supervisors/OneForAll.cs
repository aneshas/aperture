using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.Supervisors
{
    public class OneForAll : ISuperviseProjection
    {
        // TODO - Add abstract supervisor that logs and handles exceptions
        public virtual async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct) =>
            await projection.ProjectAsync(streamEvents, ct);
    }
}