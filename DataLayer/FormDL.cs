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

        public IEnumerable<Form> GetForm()
        {
            var response = conn.Query<Form>(Query.Form.getForm);
            return response;
        }
        public bool PostForm()
        {
            var response = conn.Execute(Query.Form.postForm) > 0;
            return response;
        }
    }
}
