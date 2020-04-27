using System.Collections.Generic;
using System.Threading.Tasks;
using Aperture.Core;
using Aperture.Quartz;
using FluentAssertions;
using Xunit;

namespace Aperture.Tests.Quartz
{
    public class SubscriptionTests
    {
        private readonly Subscription _subscription;

        public SubscriptionTests()
        {
            _subscription = new Subscription();
        }

        [Fact]
        public void CanFetchEnqueuedRequest()
        {
            var request = new EventBatchRequest(GetType(), 5, 100);

            _subscription.EnqueueRequest(request);

            _subscription.NextRequest()
                .Should()
                .BeEquivalentTo(request);
        }

        [Fact]
        public void EmptyRequestQueueReturnsNull()
        {
            _subscription.NextRequest()
                .Should()
                .BeNull();
        }

        [Fact]
        public void EmptyResponseQueueReturnsNull()
        {
            _subscription.DequeueResponse()
                .Should()
                .BeNull();
        }

        [Fact]
        public void EnqueueRequestBlocksUntilCurrentRequestIsDequeued()
        {
            var request = new EventBatchRequest(GetType(), 5, 100);

            _subscription.EnqueueRequest(request);

            Task.Run(async () =>
            {
                await Task.Delay(500);
                _subscription.NextRequest();
            });
            
            _subscription.EnqueueRequest(request);
        }

        [Fact]
        public void CanDequeueEnqueuedResponses()
        {
            var batch1 = new EventBatchResponse(new List<EventData>());
            var batch2 = new EventBatchResponse(null);
            var batch3 = new EventBatchResponse(new List<EventData>
            {
                new EventData()
            });

            _subscription.EnqueueResponse(batch1);
            _subscription.EnqueueResponse(batch2);
            _subscription.EnqueueResponse(batch3);

            var response1 = _subscription.DequeueResponse();
            var response2 = _subscription.DequeueResponse();
            var response3 = _subscription.DequeueResponse();

            response1.Should().BeEquivalentTo(batch1);
            response2.Should().BeEquivalentTo(batch2);
            response3.Should().BeEquivalentTo(batch3);
        }
    }
}