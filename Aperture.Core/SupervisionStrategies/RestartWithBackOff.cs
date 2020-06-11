using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.SupervisionStrategies
{
    public class RestartWithBackOff : ISuperviseProjection
    {
        // TODO Add ctor with options and use Polly - Log and handle ex
        public async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct)
        {
            await projection.ProjectAsync(streamEvents, ct);
        }
    }
}