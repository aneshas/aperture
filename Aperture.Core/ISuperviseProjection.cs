using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface ISuperviseProjection
    {
        Task Run(
            IStreamEvents streamEvents,
            IProjectEvents projection,
            Action<Exception> handleException,
            CancellationToken ct);
    }
}