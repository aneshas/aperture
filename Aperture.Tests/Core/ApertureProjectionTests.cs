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
    public class ApertureProjectionTests
    {
        [Fact]
        public async Task CanSubscribeFromProvidedOffset()
        {
            var expectedOffset = 55;
            
            var offsetTracker = new Mock<ITrackOffset>();

            offsetTracker
                .Setup(t => t.GetOffsetAsync(It.IsAny<Type>()))
                .ReturnsAsync(expectedOffset);

            var projection = new MoviesApertureProjection(offsetTracker.Object);
            
            var eventStream = new Mock<IStreamEvents>();

            await projection.ProjectAsync(eventStream.Object, CancellationToken.None);

            eventStream.Verify(
                stream => stream.SubscribeAsync(
                    typeof(MoviesApertureProjection), 
                    expectedOffset,
                    CancellationToken.None, It.IsAny<Func<EventData, Task>>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CanInvokeProperEventHandlers()
        {
            var events = new List<IEvent>
            {
                new MovieAddedToCatalogue("A movie", Genre.Crime),
                new MovieWasRated(4),
                new MovieWasRated(5),
                new MovieAddedToCatalogue("Another Movie", Genre.SciFi)
            };

            var projection = MockProjection();

            await ProjectEventsAsync(projection, events);

            projection.Events
                .Should()
                .BeEquivalentTo(events);
        }

        [Fact]
        public async Task MissingEventHandlersAreIgnored()
        {
            var events = new List<IEvent>
            {
                new MovieAddedToCatalogue("A movie", Genre.Crime),
                new MovieWasRated(4),
                new MovieWasRated(5),
                new MovieWasRemoved("Because..."),
                new MovieAddedToCatalogue("Another Movie", Genre.SciFi)
            };

            var projection = MockProjection();

            await ProjectEventsAsync(projection, events);

            projection.Events
                .Should()
                .BeEquivalentTo(events.Where(x => !(x is MovieWasRemoved)));
        }

        private MoviesApertureProjection MockProjection()
        {
            var offsetTracker = new Mock<ITrackOffset>();

            offsetTracker
                .Setup(t => t.GetOffsetAsync(It.IsAny<Type>()))
                .ReturnsAsync(0);

            return new MoviesApertureProjection(offsetTracker.Object);
        }

        private async Task ProjectEventsAsync(IProjectEvents projection, List<IEvent> events)
        {
            var eventStream = new EventStream(events);

            await projection.ProjectAsync(eventStream, CancellationToken.None);
        }
    }
}