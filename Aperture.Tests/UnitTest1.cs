using Aperture.Core.CoreAdapters;
using Xunit;

namespace Aperture.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var agent = ApertureAgentBuilder
                .BuildDefault(null)
                .AddProjection(null)
                .AddProjection(null)
                .AddProjection(null)
                .AddProjection(null)
                // Override default settings
                .UseEventStream(null)
                .UseSupervisionStrategy(new RestartWithBackOff()) // TODO - Make this one default?
                .UseCancellationTokenSource(null)
                .Configure(cfg =>
                {
                    // TODO Use methods
                    // Figure out what to configure here
                    // timeouts
                    // pull interval
                    // exception handlers
                    // exception loggers
                    // retry / delay config
                })
                // Run it
                .Start(); // TODO this should not block

            agent.Stop();
        }
    }
}