using System;

namespace Aperture.Quartz
{
    public class EventBatchRequest
    {
        public Type Projection { get; }
        
        public int FromOffset { get; }
        
        public int BatchSize { get; }

        public EventBatchRequest(Type projection, int fromOffset, int batchSize)
        {
            Projection = projection;
            FromOffset = fromOffset;
            BatchSize = batchSize;
        }
    }
}