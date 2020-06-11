using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Example.Events;
using Aperture.sql;
using Microsoft.Extensions.Logging;

namespace Aperture.Example.Projections
{
    public class CrimeMovieProjection : SqlProjection, IHandleEvent<MovieAddedToCatalogue>
    {
        private readonly ILogger<Worker> _logger;

        public CrimeMovieProjection(ITrackOffset offsetTracker, ILogger<Worker> logger) : base(offsetTracker)
        {
            _logger = logger;
            _logger.LogInformation($"Starting {GetType().Name}...");
        }

        public Task HandleAsync(MovieAddedToCatalogue @event)
        {
            if (@event.Genre != Genre.Crime) return Task.CompletedTask;

            _logger.LogInformation($"Saving {@event.Title}.");

            return Task.CompletedTask;
        }
    }
}