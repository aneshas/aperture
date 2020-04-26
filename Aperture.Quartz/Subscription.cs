using System;
using System.Collections.Concurrent;

namespace Aperture.Quartz
{
    public class Subscription
    {
        private readonly ConcurrentQueue<EventBatchRequest> _requestQueue;
        private readonly ConcurrentQueue<EventBatchResponse> _responseQueue;

        public Subscription(
            ConcurrentQueue<EventBatchRequest> requestQueue,
            ConcurrentQueue<EventBatchResponse> responseQueue)
        {
            _requestQueue = requestQueue;
            _responseQueue = responseQueue;
        }

        public EventBatchRequest NextRequest()
        {
            throw new NotImplementedException();
        }

        public void EnqueueRequest(EventBatchRequest request)
        {
            throw new NotImplementedException();
        }

        public void EnqueueResponse(EventBatchResponse response)
        {
            throw new NotImplementedException();
        }
    }
}