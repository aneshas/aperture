using System;
using System.Threading.Tasks;
using Aperture.Core;

namespace Aperture.Sql
{
    public class SqlOffsetTracker : ITrackOffset
    {
        // TODO - We need sql connection 
        // TODO - Create table if not exists - upon construction?

        public Task SaveOffsetAsync(Type projection, int currentOffset)
        {
            // TODO - Upsert
            throw new NotImplementedException();
        }

        public Task<int> GetOffsetAsync(Type projection)
        {
            throw new NotImplementedException();
        }
    }
}