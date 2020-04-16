﻿using System.Threading;
using System.Threading.Tasks;

namespace Aperture.Core.CoreAdapters
{
    public class RestartWithBackOff : ISupervisionStrategy
    {
        public async Task RunProjection(IEventStream eventStream, ApertureProjection projection, CancellationToken ct)
        {
            await projection.Project(eventStream, ct);
        }
    }
}