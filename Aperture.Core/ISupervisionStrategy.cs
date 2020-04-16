using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface ISupervisionStrategy
    {
        Task RunProjection(IEventStream eventStream, ApertureProjection projection,  CancellationToken ct);
    }
}