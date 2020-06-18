using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Tests.Core.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Aperture.Tests.Core
{
    public class ApertureAgentTests
    {
        [Fact]
        public async Task Default_Configuration_Projects_Events()
        {
            var sciFiEvents = new List<IEvent>
            {
                new MovieAddedToCatalogue("A third movie", Genre.SciFi),
                new MovieAddedToCatalogue("Fourth movie", Genre.SciFi),
            };

            var crimeEvents = new List<IEvent>
            {
                new MovieAddedToCatalogue("A movie", Genre.Crime),
                new MovieAddedToCatalogue("A second movie", Genre.Crime),
                new MovieAddedToCatalogue("Another movie", Genre.Crime)
            };

            var eventStream = new PullEventStream(
                new EventStore(
                    sciFiEvents.Concat(crimeEvents).ToList()
                ),
                new PullEventStream.Config
                {
                    PullInterval = TimeSpan.FromMilliseconds(10)
                }
            );
            
            var offsetTracker = new Mock<ITrackOffset>();

            offsetTracker
                .Setup(t => t.GetOffsetAsync(It.IsAny<Type>()))
                .ReturnsAsync(0);

            var sciFiProjection = new SciFiMoviesProjection(offsetTracker.Object);
            var crimeProjection = new CrimeMoviesProjection(offsetTracker.Object);

            var cts = new CancellationTokenSource(2000);

            await Assert.ThrowsAsync<TaskCanceledException>(
                async () => await ApertureAgentBuilder
                    .CreateDefault()
                    .UseCancellationTokenSource(cts)
                    .AddProjection(sciFiProjection)
                    .AddProjection(crimeProjection)
                    .UseEventStream(eventStream)
                    .StartAsync());

            sciFiProjection.Events
                .Should()
                .BeEquivalentTo(sciFiEvents);

            crimeProjection.Events
                .Should()
                .BeEquivalentTo(crimeEvents);
        }
        
        // TODO - Add more "load" tests
    }
}