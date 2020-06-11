using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aperture.Core;
using Aperture.Core.SupervisionStrategies;
using Aperture.Tests.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Aperture.Tests
{
    public class ApertureAgentTests
    {
        [Fact]
        public void Default_Configuration_Projects_Events()
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
            
            var eventStore = new EventStore(
                sciFiEvents.Concat(crimeEvents).ToList() 
            );

            var offsetTracker = new Mock<ITrackOffset>();

            offsetTracker
                .Setup(t => t.GetOffsetAsync(It.IsAny<Type>()))
                .ReturnsAsync(0);

            var sciFiProjection = new SciFiMoviesProjection(offsetTracker.Object);
            var crimeProjection = new CrimeMoviesProjection(offsetTracker.Object);

            // TODO - Figure out how to complete on success 
            var cts = new CancellationTokenSource(1000);

            // TODO - Add mock event stream
            ApertureAgentBuilder
                .CreateDefault()
                .UseCancellationTokenSource(cts)
                .AddProjection(sciFiProjection)
                .AddProjection(crimeProjection)
                .StartAsync();

            sciFiProjection.Events
                .Should()
                .BeEquivalentTo(sciFiEvents);

            crimeProjection.Events
                .Should()
                .BeEquivalentTo(crimeEvents);
        }
        
        // TODO - Test cancellation
        // TODO - Add more "load" tests

        [Fact]
        public void ReferenceTest()
        {
            // TODO - Write first integration test - make it simple

            var agent = ApertureAgentBuilder
                .CreateDefault()
                .AddProjection(new CrimeMoviesProjection(null))
                // Override default settings
                .UseEventStream(null)
                .UseSupervisor(new RestartWithBackOff()) // TODO - Make this one default?
                .UseRestartWithBackOffSupervision()
                .UseCancellationTokenSource(null);

            agent.StartAsync();
            agent.Stop();
        }
    }
}