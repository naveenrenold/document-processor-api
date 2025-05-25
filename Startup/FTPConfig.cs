using FluentFTP;
using Microsoft.Extensions.Options;

namespace DocumentProcessor.Startup
{
    public static class FTPConfig
    {
        public static void AddFtpConnection(this WebApplicationBuilder builder, string ftpServer, string ftpUser, string ftpPassword)
        {
            builder.Services.AddSingleton<IFtpClient>(new FtpClient(ftpServer, ftpUser, ftpPassword));
        }
    }
}
