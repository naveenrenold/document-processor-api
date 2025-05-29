using Dapper;
using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using DocumentProcessor.Startup;
using FluentFTP;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using Query = DocumentProcessor.Constants.Query;

namespace DocumentProcessor.DataLayer
{
    public class FormDL(IDbConnection dbconnection, IFtpClient ftpClient, IOptions<AppSettings> appSettings) : BaseRepository, IFormDL
    {
        private readonly IDbConnection conn = dbconnection;

        public async Task<IEnumerable<Form>> GetForm(FormFilter filter)
        {
            var query = Query.Form.getForm;
            var form = new DynamicParameters();
            query = BuildQuery(filter, query, ref form);
            return await conn.QueryAsync<Form>(query, form);            
        }
        public async Task<int> PostForm(Form request, IFormFileCollection? attachments)
        {
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    request.StatusId = 2;
                    var formId = await connection.ExecuteScalarAsync<int>(Query.Form.postForm, request, transaction);

                    if (attachments == null)
                    {
                        transaction.Commit();
                        return formId;
                    }
                    try
                    {
                        ftpClient.AutoConnect();
                        foreach (var attachment in attachments)
                        {
                            var fileName = $"{attachment.FileName}";
                            var destinationPath = $"{appSettings.Value.FTPDestinationPath}/{formId}/{fileName}";

                            var attachmentData = new Attachment
                            {
                                Id = formId,
                                FileName = fileName,
                                FileType = attachment.ContentType,
                                FilePath = destinationPath,
                                UploadedBy = request.LastUpdatedBy,
                            };

                            var result = await connection.ExecuteAsync(Query.Attachment.addAttachment, attachmentData, transaction);
                            if(!ftpClient.DirectoryExists($"{appSettings.Value.FTPDestinationPath}/{formId}"))
                            {
                                ftpClient.CreateDirectory($"{appSettings.Value.FTPDestinationPath}/{formId}");
                            }
                            using (var stream = attachment.OpenReadStream())
                            {
                                ftpClient.UploadStream(stream, destinationPath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Failed to upload attachments", ex);
                    }

                    transaction.Commit();
                    return formId;
                }
            }
        }
    }
}
