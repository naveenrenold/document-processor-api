using Microsoft.Data.SqlClient;
using System.Data;

namespace DocumentProcessor.Startup
{
    public static class SQLConfig
    {
        public static void AddSqlConnection(this WebApplicationBuilder builder, string connectionString)
        {
            builder.Services.AddTransient<IDbConnection>((serviceProvider) => new SqlConnection(connectionString));
        }
    }
}
