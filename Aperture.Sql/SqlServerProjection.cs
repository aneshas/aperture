using System;
using System.Threading.Tasks;
using System.Transactions;
using Aperture.Core;

namespace Aperture.Sql
{
    // TODO - This should maybe be in the core package and renamed to Transactional Projection
    // Or maybe it's still better to leave it here since transactionality would probably mostly
    // be used with sql persistence anyway
    // Since this is the case offsets + read model itself should be saved in the sam db
    // in order to avoid distributed transactions which means that these projections should use their own 
    // specific offset trackers (eg. mssql, postgres etc...)
    public class SqlServerProjection : Projection
    {
        private readonly ITrackOffset _offsetTracker;

        private readonly TransactionOptions _txOptions
            = new TransactionOptions
            {
                // TODO This should probably be higher since it affects event handler also. Or configurable?
                // Is this even needed since we use separate tables for each projection
                IsolationLevel = IsolationLevel.RepeatableRead
            };

        protected SqlServerProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
        {
            using var txScope = new TransactionScope(
                TransactionScopeOption.Required,
                _txOptions,
                TransactionScopeAsyncFlowOption.Enabled);

            await base.TrackAndHandleEventAsync(projection, eventData);

            txScope.Complete();
        }
    }
}