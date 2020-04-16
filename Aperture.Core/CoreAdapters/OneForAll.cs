using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.CoreAdapters
{
    public class OneForAll : ISupervisionStrategy
    {
        public virtual async Task RunProjection(ApertureProjection projection, CancellationToken ct) =>
            await projection.Project(ct);
    }
}