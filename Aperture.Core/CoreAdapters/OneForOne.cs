using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.CoreAdapters
{
    public class OneForOne : OneForAll
    {
        public override async Task RunProjection(
            ApertureProjection projection,
            IEventStream eventStream,
            CancellationToken ct)
        {
            try
            {
                await base.RunProjection(projection, eventStream, ct);
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