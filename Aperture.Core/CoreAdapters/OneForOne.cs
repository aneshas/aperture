using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.CoreAdapters
{
    public class OneForOne : OneForAll
    {
        public override async Task RunProjection(ApertureProjection projection, CancellationToken ct)
        {
            try
            {
                await base.RunProjection(projection, ct);
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