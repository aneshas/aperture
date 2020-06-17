using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IProjectEvents
    {
        Task ProjectAsync(IStreamEvents streamEvents, CancellationToken ct);
    }
}