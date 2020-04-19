using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.SupervisionStrategies
{
    public class OneForOne : OneForAll
    {
        public override async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct)
        {
            try
            {
                await base.Run(streamEvents, projection, ct);
            }
            catch (Exception e)
            {
                // TODO - Restart immediately
                Console.WriteLine(e);
                throw;
            }
        }
    }
}