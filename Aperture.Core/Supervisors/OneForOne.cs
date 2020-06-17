using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.Supervisors
{
    public class OneForOne : OneForAll
    {
        public override async Task Run(
            IStreamEvents streamEvents,
            IProjectEvents projection,
            Action<Exception> handleException,
            CancellationToken ct)
        {
            while (true)
            {
                try
                {
                    await base.Run(streamEvents, projection, handleException, ct);
                }
                catch (Exception e)
                {
                    handleException(e);
                    // TODO - Add max restarts and check max restart count (per projection) 
                }
            }
        }
    }
}