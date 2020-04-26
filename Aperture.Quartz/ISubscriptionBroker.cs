using System.Collections.Generic;

namespace Aperture.Quartz
{
    public interface ISubscriptionBroker
    {
        IEnumerable<Subscription> Subscriptions();
    }
}