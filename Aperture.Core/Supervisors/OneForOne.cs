using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.Supervisors
{
    public class OneForOne : OneForAll
    {
        public override async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct)
        {
            while (true)
            {
                try
                {
                    await base.Run(streamEvents, projection, ct);
                }
                catch (Exception)
                {
                    // TODO - Check max restart count 
                }
            }
        }
    }
}