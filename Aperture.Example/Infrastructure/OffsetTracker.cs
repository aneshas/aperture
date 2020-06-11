using System;
using System.Threading.Tasks;
using Aperture.Core;
using Microsoft.Extensions.Logging;

namespace Aperture.Example.Infrastructure
{
    public class OffsetTracker : ITrackOffset
    {
        private readonly ILogger<Worker> _logger;

        public OffsetTracker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public Task SaveOffsetAsync(Type projection, int currentOffset)
        {
           _logger.LogInformation("Saving offset..."); 
           
           return Task.CompletedTask;
        }

        public Task<int> GetOffsetAsync(Type projection)
        {
           _logger.LogInformation("Fetching offset..."); 
           
           return Task.FromResult(0);
        }
    }
}