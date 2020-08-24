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
        // TODO - Retry aperture projection exception by default, but add ability to override
        // (figure out which approach makes sense)
        
        private readonly Config _config = new Config();

        public RestartWithBackOff()
        {
        }

        public RestartWithBackOff(Config config)
        {
            _config = config;
        }

        public async Task Run(
            IStreamEvents streamEvents, 
            IProjectEvents projection,
            Action<Exception> handleException,
            CancellationToken ct)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: _config.FirstRetryDelay,
                retryCount: _config.RetryCount);

            await Policy
                .Handle<ApertureProjectionException>()
                .WaitAndRetryAsync(delay, (ex, _) => handleException(ex))
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