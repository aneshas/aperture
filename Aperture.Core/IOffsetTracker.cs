using System;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface IOffsetTracker
    {
        Task SaveOffsetAsync(Type projection, int currentOffset);

        Task<int> GetOffsetAsync(Type projection);
    }
}