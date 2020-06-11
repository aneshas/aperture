using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aperture.Example
{
    public class Worker : BackgroundService
    {
        private readonly ApertureAgent _apertureAgent;
        
        private readonly ILogger<Worker> _logger;

        public Worker(ApertureAgent apertureAgent, ILogger<Worker> logger)
        {
            _apertureAgent = apertureAgent;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Aperture Agent...");

            await _apertureAgent
                .UseCancellationToken(stoppingToken)
                .StartAsync();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Aperture Agent...");
            
            _apertureAgent.Stop();

            await Task.Delay(2000, cancellationToken);
        }
    }
}