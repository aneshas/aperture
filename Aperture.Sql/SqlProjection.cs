﻿using System;
using System.Threading.Tasks;
using System.Transactions;
using Aperture.Core;

namespace Aperture.Sql
{
    public class SqlProjection : ApertureProjection
    {
        private readonly ITrackOffset _offsetTracker;

        private readonly TransactionOptions _txOptions
            = new TransactionOptions
            {
                // TODO This should probably be higher since it affects event handler also. Or configurable?
                IsolationLevel = IsolationLevel.RepeatableRead
            };

        protected SqlProjection(ITrackOffset offsetTracker) : base(offsetTracker)
        {
            _offsetTracker = offsetTracker;
        }

        protected override async Task TrackAndHandleEventAsync(Type projection, EventData eventData)
        {
            using var txScope = new TransactionScope(
                TransactionScopeOption.Required,
                _txOptions,
                TransactionScopeAsyncFlowOption.Enabled);
            
            await HandleEventAsync(eventData.Event);
            await _offsetTracker.SaveOffsetAsync(projection, eventData.Offset);

            txScope.Complete();
        }
    }
}