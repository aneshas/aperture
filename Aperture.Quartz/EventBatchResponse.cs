using System.Collections.Generic;
using System.Linq;
using Aperture.Core;

namespace Aperture.Quartz
{
    public class EventBatchResponse
    {
         public IEnumerable<EventData> EventBatch { get; }

         public EventBatchResponse(IEnumerable<EventData> eventBatch)
         {
             EventBatch = eventBatch;
         }

         public bool IsEmpty => EventBatch == null;
    }
}