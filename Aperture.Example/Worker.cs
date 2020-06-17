using System;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Core.Supervisors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aperture.Example
{
    public class Worker : IHostedService
    {
        private readonly ApertureAgent _apertureAgent;

        private readonly ILogger<Worker> _logger;

        private readonly IHostApplicationLifetime _applicationLifetime;

        public Worker(ApertureAgent apertureAgent, ILogger<Worker> logger, IHostApplicationLifetime applicationLifetime)
        {
            _apertureAgent = apertureAgent;
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _apertureAgent
                    .UseLogger(_logger)
                    .UseCancellationToken(cancellationToken)
                    .StartAsync();
            }
            catch (Exception)
            {
                _applicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _apertureAgent.Stop();
            return Task.CompletedTask;
        }
    }
}