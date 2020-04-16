using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Aperture.Core.CoreAdapters.Projections
{
    public class TxProjection : ApertureProjection
    {
        private readonly ITrackOffset _offsetTracker;

        private readonly TransactionOptions _txOptions
            = new TransactionOptions
            {
                // TODO This should probably be higher since it affects event handler also. Or configurable.
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

        public TxProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
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