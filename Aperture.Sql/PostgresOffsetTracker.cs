using System;
using System.Data;
using System.Threading.Tasks;
using Aperture.Core;
using Dapper;
using Npgsql;

namespace Aperture.Sql
{
    public class PostgresOffsetTracker : ITrackOffset
    {
        // TODO - We need sql connection 
        // TODO - Create table if not exists - upon construction?

        public PostgresOffsetTracker(string connString)
        {
            var conn = new NpgsqlConnection(connString);
            conn.Open();
            
            var sql = "";
            
            // TODO - Use table per projection so we don't have locking issues
            // only initialize connection in constructor 
            
            conn.Execute(sql);
        }

        public Task SaveOffsetAsync(Type projection, int currentOffset)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOffsetAsync(Type projection)
        {
            // TODO - Create table if not exists - then cache it  
            throw new NotImplementedException();
        }
    }
}