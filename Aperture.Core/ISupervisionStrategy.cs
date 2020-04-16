using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface ISupervisionStrategy
    {
        Task RunProjection( ApertureProjection projection, IEventStream eventStream, CancellationToken ct);
    }
}