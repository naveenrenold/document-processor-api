namespace DocumentProcessor.Constants.Query
{
    public static class Activity
    {
        public static readonly string getActivity = @"With CTE AS
        (
        SELECT 
        A.ActivityId, A.Id, AT.ActivityTypeName, A.Comments, A.CreatedBy, A.CreatedOn, A.Field, A.OldValue, A.NewValue
        FROM [Activity] A
        inner join ActivityType AT on A.ActivityTypeId = AT.ActivityTypeId )
        select count(1) OVER() AS 
        TotalRecords, {2}
        from CTE {0} {1}
        OFFSET @offset ROWS 
        FETCH NEXT @limit ROWS ONLY;        
        ";

        public static readonly string postActivity = @"
        DECLARE @LocalDateTime DATETIME;
        exec getLocalDate @LocalDateTime OUTPUT;
        INSERT INTO activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
            VALUES (@Id, @ActivityTypeId, @Comments, @CreatedBy, @LocalDateTime, @Field, @OldValue, @NewValue);
        ";

    }
}
