using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Aperture.Core.CoreAdapters
{
    public class Projection : ApertureProjection
    {
        private readonly IOffsetTracker _offsetTracker;

        private readonly TransactionOptions _txOptions
            = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

        public Projection(IOffsetTracker offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task HandleEventAsync(Type projection, EventData eventData)
        {
            using (var txScope = new TransactionScope(
                TransactionScopeOption.Required, 
                _txOptions, 
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await HandleEventAsync(eventData.Event);
                await _offsetTracker.SaveOffsetAsync(projection, eventData.Offset);
                
                txScope.Complete();
            }
        }
    }
}