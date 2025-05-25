namespace DocumentProcessor.Startup
{
    public class AppSettings
    {
        public required string[] AllowedUrls { get; set; }
        public required string FTPServer { get; set; }
        public required string FTPUsername { get; set; }
        public required string FTPPassword { get; set; }
        public required string FTPDestinationPath { get; set; }
    }
}
