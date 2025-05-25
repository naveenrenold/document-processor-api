using Dapper;
using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using System.Data;
using Query = DocumentProcessor.Constants.Query;

namespace DocumentProcessor.DataLayer
{
    public class FormDL(IDbConnection dbconnection) : IFormDL
    {
        private readonly IDbConnection conn = dbconnection;

        public async Task<IEnumerable<Form>> GetForm()
        {
            return await conn.QueryAsync<Form>(Query.Form.getForm);            
        }
        public async Task<int> PostForm(Form request, List<Attachment> attachments)
        {
            request.StatusId = 2;
            return await conn.ExecuteScalarAsync<int>(Query.Form.postForm, request);                           
        }
    }
}
