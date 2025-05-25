namespace DocumentProcessor.Constants.Query
{
    public static class Attachment
    {
        public static readonly string addAttachment = @"
        DECLARE @LocalDateTime DATETIME;
        exec getLocalDate @LocalDateTime OUTPUT;

        Insert into Attachment(Id, FileName, FilePath, FileType, UploadedBy, UploadedOn) values(
        @Id, @FileName, @FilePath, @FileType, @UploadedBy, @LocalDateTime)
    ";
    }
}
