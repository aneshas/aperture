using System;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Core.SupervisionStrategies;
using Aperture.Example.Projections;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aperture.Example
{
    public class Worker : BackgroundService
    {
        private ApertureAgent _apertureAgent;
        
        private readonly ILogger<Worker> _logger;

        private readonly IStreamEvents _eventStream;
        
        private readonly SciFiMoviesProjection _sciFiMoviesProjection;

        private readonly CrimeMovieProjection _crimeMovieProjection;

        public Worker(
            ILogger<Worker> logger,
            SciFiMoviesProjection sciFiMoviesProjection,
            IStreamEvents eventStream,
            CrimeMovieProjection crimeMovieProjection)
        {
            _logger = logger;
            _sciFiMoviesProjection = sciFiMoviesProjection;
            _eventStream = eventStream;
            _crimeMovieProjection = crimeMovieProjection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _apertureAgent = ApertureAgentBuilder
                .CreateDefault()
                .AddProjection(_sciFiMoviesProjection)
                .AddProjection(_crimeMovieProjection)
                .UseEventStream(_eventStream) 
                .UseCancellationToken(stoppingToken)
                .Configure(cfg => { });

            _logger.LogInformation("Aperture agent started projecting at: {time}", DateTimeOffset.Now);
            
            await _apertureAgent.StartAsync();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _apertureAgent.Stop();

            await Task.Delay(2000, cancellationToken);
        }
    }
}