using System.Data;
using Query = DocumentProcessor.Constants.Query;
using Dapper;
using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer
{
    public class LocationDL(IDbConnection conn) : ILocationDL
    {
        public async Task<IEnumerable<Location>> GetLocation()
        {
            return await conn.QueryAsync<Location>(Query.Location.GetLocation);
        }
    }
}
