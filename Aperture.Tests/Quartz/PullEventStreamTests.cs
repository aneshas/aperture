using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Quartz;
using FluentAssertions;
using Moq;
using Quartz;
using Xunit;

using PullEventStream = Aperture.Quartz.PullEventStream;
using IEventStore = Aperture.Quartz.IEventStore;

namespace Aperture.Tests.Quartz
{
    public class PullEventStreamTests
    {
        private CancellationTokenSource _cts; 

        private readonly Mock<IEventStore> _eventStore;

        private readonly PullEventStream _pullEventStream;

        private readonly IJob _job;

        public PullEventStreamTests()
        {
            _eventStore = new Mock<IEventStore>();
            _pullEventStream = new PullEventStream();
            _job = new LoadEventsJob(_eventStore.Object, _pullEventStream);
        }

        [Fact]
        public async Task Test()
        {
            var projection = typeof(FooProjection);

            var sourceEvents = new List<string> {"first", "second", "third"};

            _eventStore
                .Setup(x => x.LoadEventsAsync(projection, 0, 100))
                .ReturnsAsync(new[]
                {
                    new EventData
                    {
                        Event = sourceEvents[0]
                    },
                });

            _eventStore
                .Setup(x => x.LoadEventsAsync(projection, 100, 100))
                .ReturnsAsync(new[]
                {
                    new EventData
                    {
                        Event = sourceEvents[1]
                    },
                });

            _eventStore
                .Setup(x => x.LoadEventsAsync(projection, 200, 100))
                .ReturnsAsync(new[]
                {
                    new EventData
                    {
                        Event = sourceEvents[2]
                    },
                });

            var receivedEvents = new List<string>();
            
            _cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));

            await Task.Run(() => _pullEventStream.SubscribeAsync(
                projection,
                0,
                _cts.Token, eventData =>
                {
                    receivedEvents.Add((string) eventData.Event);
                    return Task.CompletedTask;
                }));

            Thread.Sleep(100);

            await _job.Execute(null);
            await _job.Execute(null);
            await _job.Execute(null);

            receivedEvents
                .Should()
                .BeEquivalentTo(sourceEvents);
        }
    }
}