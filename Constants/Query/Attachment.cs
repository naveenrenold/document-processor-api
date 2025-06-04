namespace DocumentProcessor.Constants.Query
{
    public static class Attachment
    {
        public static readonly string getAttachment = @"
        With CTE AS
        (
        SELECT 
            A.[AttachmentId],
            A.[Id],
            A.[FileName],
            A.[FilePath],
            A.[FileSize],
            A.[FileType],
            A.[UploadedBy],
            A.[UploadedOn]
            from Attachment A
         )
        select count(1) OVER() AS 
        TotalRecords, {2}
        from CTE {0} {1}
        OFFSET @offset ROWS 
        FETCH NEXT @limit ROWS ONLY;";

        public static readonly string addAttachment = @"
        DECLARE @LocalDateTime DATETIME;
        exec getLocalDate @LocalDateTime OUTPUT;

        Insert into Attachment(Id, FileName, FilePath, FileSize, FileType, UploadedBy, UploadedOn) values(
        @Id, @FileName, @FilePath, @FileSize, @FileType, @UploadedBy, @LocalDateTime)
    ";

        
        public static readonly string deleteAttachment = @"
        DECLARE @LocalDateTime DATETIME;
        exec getLocalDate @LocalDateTime OUTPUT;
        
        Delete from Attachment where Id = @Id and FileName in @FileNames
    ";
    }
}
