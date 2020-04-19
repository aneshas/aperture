using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.SupervisionStrategies
{
    public class OneForAll : ISuperviseProjection
    {
        public virtual async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct) =>
            await projection.ProjectAsync(streamEvents, ct);
    }
}