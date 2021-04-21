using System;
using System.Data;
using System.Threading.Tasks;
using Aperture.Core;
using Dapper;

namespace Aperture.Sql
{
    // TODO - Can this one work with EF or would we need an Ef specific tracker (how do transactions behave?)
    internal class SqlServerOffsetTracker : ITrackOffset
    {
        private readonly IDbConnection _conn;

        public SqlServerOffsetTracker(IDbConnection conn)
        {
            _conn = conn;
            conn.Open();
            
            var sql = "";
            
            // TODO - Use table per projection so we don't have locking issues
            
            conn.Execute(sql);
        }

        public Task SaveOffsetAsync(Type projection, int currentOffset)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOffsetAsync(Type projection)
        {
            CreateTrackingTableFor(projection);
            throw new NotImplementedException();
        }

        private void CreateTrackingTableFor(Type projection)
        {
            var tableName = TableNameFor(projection);
            
            // - create if not exists return
            // - tx
            // - Create
            // - Insert entry 
            // - end
            // - cache
        }
        
        private static string TableNameFor(Type projection) =>
             $"Aperture_{nameof(SqlServerOffsetTracker)}_For_{projection.Name}";
    }
}