using Dapper;
using DocumentProcessor.DataLayer.Interface;
using DocumentProcessor.Model;
using DocumentProcessor.Startup;
using FluentFTP;
using Microsoft.Extensions.Options;
using System.Data;
using Query = DocumentProcessor.Constants.Query;

namespace DocumentProcessor.DataLayer
{
    public class AttachmentDL(IDbConnection dbconnection, IFtpClient ftpClient, IOptions<AppSettings> appSettings) : BaseRepository, IAttachmentDL
    {
        private readonly IDbConnection conn = dbconnection;

        public async Task<IEnumerable<Attachment>> GetAttachment(QueryFilter<AttachmentResponse> filter, bool? downloadAttachment = true)
        {
            var query = Query.Attachment.getAttachment;
            var attachment = new DynamicParameters();
            query = BuildQuery(filter, query, ref attachment);
            var attachments = await conn.QueryAsync<Attachment>(query, attachment);
            if(attachments == null || !attachments.Any())
            {
                return [];
            }
            try
            {
                foreach (var item in attachments)
                {
                    var filePath = $"{appSettings.Value.FTPDestinationPath}/{item.Id}/{item.FileName}";
                    ftpClient.AutoConnect();
                    ftpClient.DownloadBytes(out byte[] file, filePath);
                    item.FileContent = file;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error downloading attachments: {ex.Message}");
            }
            return attachments;
        }

        //public async Task<bool> PostAttachment(QueryFilter<AttachmentResponse> filter)
        //{
        //    //var query = Query.Attachment.postAttachment;
        //    //var attachment = new DynamicParameters();
        //    //query = BuildQuery(filter, query, ref attachment);
        //    //return await conn.QueryAsync<Attachment>(query, attachment);
        //    return false;
        //}
    }
}
