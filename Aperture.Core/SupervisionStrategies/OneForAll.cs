using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.SupervisionStrategies
{
    public class OneForAll : ISuperviseProjection
    {
        // TODO - Log and handle ex
        public virtual async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct) =>
            await projection.ProjectAsync(streamEvents, ct);
    }
}