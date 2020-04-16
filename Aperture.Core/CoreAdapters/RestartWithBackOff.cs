using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.CoreAdapters
{
    public class RestartWithBackOff : ISupervisionStrategy
    {
        public async Task RunProjection(ApertureProjection projection, IEventStream eventStream, CancellationToken ct)
        {
            await projection.Project(eventStream, ct);
        }
    }
}