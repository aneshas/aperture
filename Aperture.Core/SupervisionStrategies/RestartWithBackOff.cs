using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.SupervisionStrategies
{
    public class RestartWithBackOff : ISuperviseProjection
    {
        // TODO Add ctor with options and use Polly - Log and handle ex - is there a more lightweight library?
        // https://github.com/Polly-Contrib/Polly.Contrib.WaitAndRetry#wait-and-retry-with-jittered-back-off
        
        public async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct)
        {
            await projection.ProjectAsync(streamEvents, ct);
        }
    }
}