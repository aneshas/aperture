using System.Collections.Concurrent;

namespace Aperture.Quartz
{
    public class Subscription
    {
        private const int RequestQueueCapacity = 1;

        private readonly BlockingCollection<EventBatchRequest> _requestQueue
            = new BlockingCollection<EventBatchRequest>(RequestQueueCapacity);

        private readonly ConcurrentQueue<EventBatchResponse> _responseQueue
            = new ConcurrentQueue<EventBatchResponse>();

        public EventBatchRequest NextRequest() =>
            _requestQueue.TryTake(out var request)
                ? request
                : null;

        public void EnqueueRequest(EventBatchRequest request) =>
            _requestQueue.Add(request);

        public void EnqueueResponse(EventBatchResponse response) =>
            _responseQueue.Enqueue(response);

        public EventBatchResponse DequeueResponse() =>
            _responseQueue.TryDequeue(out var response)
                ? response
                : null;
    }
}