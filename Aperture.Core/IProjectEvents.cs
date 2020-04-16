using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IProjectEvents
    {

        Task Project(IStreamEvents streamEvents, CancellationToken ct);
    }
}