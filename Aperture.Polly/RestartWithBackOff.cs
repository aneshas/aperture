using System;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Aperture.Polly
{
    public class RestartWithBackOff : ISuperviseProjection
    {
        private readonly Config _config = new Config();

        public RestartWithBackOff()
        {
        }

        public RestartWithBackOff(Config config)
        {
            _config = config;
        }

        public async Task Run(IStreamEvents streamEvents, IProjectEvents projection, CancellationToken ct)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: _config.FirstRetryDelay,
                retryCount: _config.RetryCount);

            await Policy
                .Handle<Exception>() // TODO - Do we want to retry all exceptions
                .WaitAndRetryAsync(delay)
                .ExecuteAsync(
                    async () => await projection.ProjectAsync(streamEvents, ct));
        }

        public class Config
        {
            public TimeSpan FirstRetryDelay { get; set; } = TimeSpan.FromMilliseconds(200);

            public int RetryCount { get; set; } = 100;
        }
    }
}