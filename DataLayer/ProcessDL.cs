using System.Data;
using Query = DocumentProcessor.Constants.Query;
using Dapper;
using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer
{
    public class ProcessDL(IDbConnection conn) : IProcessDL
    {        
        public async Task<IEnumerable<Process>> GetProcess()
        {
            return await conn.QueryAsync<Process>(Query.Process.GetProcess);
        }
    }
}
