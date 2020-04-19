using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aperture.Core;
using Aperture.Tests.Mocks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Aperture.Tests
{
    public class ApertureProjectionTests
    {
        [Fact]
        public void CanCallEventHandler()
        {
            var events = new List<IEvent>
            {
                new MovieAddedToCatalogue("A movie", Genre.Crime)
            };
            
            var eventStream = new EventStream(events);

            var offsetTracker = new Mock<ITrackOffset>();

            offsetTracker
                .Setup(t => t.GetOffsetAsync(It.IsAny<Type>()))
                .ReturnsAsync(0);

            var projection = new MoviesApertureProjection(offsetTracker.Object);

            projection.Project(eventStream, new CancellationToken());

            projection.Events
                .OfType<MovieAddedToCatalogue>()
                .Should()
                .SatisfyRespectively(e =>
                {
                    e.Genre.Should().Be(Genre.Crime);
                    e.Title.Should().Be("A movie");
                });
        }
    }
}