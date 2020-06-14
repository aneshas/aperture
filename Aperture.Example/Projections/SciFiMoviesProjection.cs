using System;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Example.Events;
using Aperture.Sql;
using Microsoft.Extensions.Logging;

namespace Aperture.Example.Projections
{
    public class SciFiMoviesProjection : SqlProjection, IHandle<MovieAddedToCatalogue>
    {
        private readonly ILogger<Worker> _logger;

        public SciFiMoviesProjection(ITrackOffset offsetTracker, ILogger<Worker> logger) : base(offsetTracker)
        {
            _logger = logger;
            _logger.LogInformation($"Starting {GetType().Name}...");
        }

        public Task HandleAsync(MovieAddedToCatalogue @event)
        {
            _logger.LogError(">>>>>>>>>>>>>>>>>>>>>>>> Restarting...");
            throw new NotImplementedException();
            
            // if (@event.Genre != Genre.SciFi) return Task.CompletedTask;
            //
            // _logger.LogInformation($"Saving {@event.Title}.");
            //
            // return Task.CompletedTask;
        }
    }
}