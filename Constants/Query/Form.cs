namespace DocumentProcessor.Constants.Query
{
    public static class Form
    {
        public static readonly string getForm = @"SELECT 
    [Id],
    [TypeId],
    [StatusId],
    [ProcessId],
    [CustomerName],
    [CustomerAddress],
    [LocationId],
    [CreatedBy],
    [CreatedOn],
    [LastUpdatedBy],
    [LastUpdatedOn]
FROM [Form]";

        public static readonly string postForm = @"DECLARE @LocalDateTime DATETIME,
@LocationId int;
exec getLocalDate @LocalDateTime OUTPUT;

Select @LocationId = LocationId from Location where LocationName = @Location;

Insert into Form(TypeId, StatusId, ProcessId, CustomerName, CustomerAddress,LocationId,CreatedBy,CreatedOn, LastUpdatedBy, LastUpdatedOn)
values
(@TypeId, @StatusId, @ProcessId, @CustomerName, @CustomerAddress, @LocationId, @LastUpdatedBy, @LocalDateTime, @LastUpdatedBy, @LocalDateTime);
Select @@Identity
";
    }
}
