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
    public class FormDL(IDbConnection dbconnection, IFtpClient ftpClient, IOptions<AppSettings> appSettings, IAttachmentDL attachmentDL) : BaseRepository, IFormDL
    {
        private readonly IDbConnection conn = dbconnection;

        public async Task<IEnumerable<FormResponse>> GetForm(QueryFilter filter)
        {
            var query = Query.Form.getForm;
            var form = new DynamicParameters();
            query = BuildQuery(filter, query, ref form);
            return await conn.QueryAsync<FormResponse>(query, form);            
        }
        public async Task<int> PostForm(Form request, IFormFileCollection? attachments, List<int> deleteAttachments)
        {
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    request.StatusId = request.StatusId == 0 ? 2 : request.StatusId;
                    var formId = request.Id;
                    if(formId == 0)
                    {
                        formId = await connection.ExecuteScalarAsync<int>(Query.Form.postForm, request, transaction);
                    }
                    else
                    {
                        var rowsAffected = await connection.ExecuteAsync(Query.Form.updateForm, request, transaction);
                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }

                    ftpClient.AutoConnect();                    
                    var getAttachment = await attachmentDL.GetAttachment(new QueryFilter("AttachmentId", Field: "AttachmentId,FileName", Query: $"Id eq {formId}"), false);

                    var deleteFileNames = new List<string?> { };
                    var addFileNames = attachments?.Where(a => !getAttachment.Any(b => b.FileName == a.FileName)) ?? [];
                    if (deleteAttachments != null && deleteAttachments.Count > 0)
                    {                                               
                        deleteFileNames = getAttachment.Where(a => deleteAttachments.Contains(a.AttachmentId) ).Select(x => x.FileName).ToList();                        
                        var deleteResult = await connection.ExecuteAsync(Query.Attachment.deleteAttachment, new { Id = formId, FileNames = deleteFileNames }, transaction);
                    }

                    if (attachments == null)
                    {
                        foreach (var fileName in deleteFileNames)
                        {
                            ftpClient.DeleteFile($"{appSettings.Value.FTPDestinationPath}/{formId}/{fileName}");
                        }
                        transaction.Commit();
                        return formId;
                    }
                    try
                    {                                                
                        foreach (var attachment in addFileNames)
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
                                FileSize = attachment.Length.ToString(),
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
                        foreach (var fileName in deleteFileNames)
                        {
                            ftpClient.DeleteFile($"{appSettings.Value.FTPDestinationPath}/{formId}/{fileName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Failed to update attachments", ex);
                    }

                    transaction.Commit();
                    return formId;
                }
            }
        }
    }
}
