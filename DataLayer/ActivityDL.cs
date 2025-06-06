using Dapper;
using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using System.Data;
using Query = DocumentProcessor.Constants.Query;

namespace DocumentProcessor.DataLayer
{
    public class ActivityDL(IDbConnection dbconnection) : BaseRepository, IActivityDL
    {
        private readonly IDbConnection conn = dbconnection;

        public async Task<IEnumerable<ActivityResponse>> GetActivity(QueryFilter<ActivityResponse> filter)
        {
            var query = Query.Activity.getActivity;
            var activity = new DynamicParameters();
            query = BuildQuery(filter, query, ref activity);
            return await conn.QueryAsync<ActivityResponse>(query, activity);            
        }
    }
}
