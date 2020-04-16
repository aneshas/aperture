using System;
using System.Threading.Tasks;

namespace Aperture.Core
{
    public interface ITrackOffset
    {
        Task SaveOffsetAsync(Type projection, int currentOffset);

        Task<int> GetOffsetAsync(Type projection);
    }
}