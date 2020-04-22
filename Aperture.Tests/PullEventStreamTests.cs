using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Core.EventStreams;
using Aperture.Tests.Mocks;
using Moq;
using Xunit;

namespace Aperture.Tests
{
    public class PullEventStreamTests
    {
        // Assert event store is called enough number of times with adequate params  

        [Fact]
        public async Task Test()
        {
            var batchSize = 200;
            var pullInterval = TimeSpan.FromMilliseconds(150);
            var projection = typeof(CrimeMoviesProjection);

            var initialOffset = 10;

            var lastBatchOffset = 610;
            var lastBatchSize = 100;

            var eventStore = new Mock<IEventStore>();

            eventStore
                .Setup(store => store.LoadEventsAsync(projection, It.IsAny<int>(), batchSize))
                .ReturnsAsync(GetEventData(batchSize));

            eventStore
                .Setup(store => store.LoadEventsAsync(projection, lastBatchOffset, batchSize))
                .ReturnsAsync(GetEventData(lastBatchSize));

            eventStore
                .Setup(store => store.LoadEventsAsync(projection, lastBatchOffset + lastBatchSize, batchSize))
                .ReturnsAsync((IEnumerable<EventData>) null);

            var eventStream = new PullEventStream(eventStore.Object, pullInterval, batchSize);

            var cts = new CancellationTokenSource(1000);

            // TODO Use timing provider ?

            // await eventStream.SubscribeAsync(projection, initialOffset, cts.Token, e => Task.CompletedTask);

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await eventStream.SubscribeAsync(projection, initialOffset, cts.Token, e => Task.CompletedTask));

            eventStore.Verify(
                store => store.LoadEventsAsync(projection, initialOffset, batchSize),
                Times.Once);

            eventStore.Verify(
                store => store.LoadEventsAsync(projection, 210, batchSize),
                Times.Once);

            eventStore.Verify(
                store => store.LoadEventsAsync(projection, 410, batchSize),
                Times.Once);

            eventStore.Verify(
                store => store.LoadEventsAsync(projection, lastBatchOffset, batchSize),
                Times.Once);

            eventStore.Verify(
                store => store.LoadEventsAsync(projection, lastBatchOffset + lastBatchSize, batchSize),
                Times.Once);
        }

        private IEnumerable<EventData> GetEventData(int count) =>
            Enumerable
                .Range(1, count)
                .Select(x => new EventData
                    {
                        Offset = x,
                        Event = x
                    }
                );
    }
}