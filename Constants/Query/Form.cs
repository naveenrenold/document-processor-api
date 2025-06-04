namespace DocumentProcessor.Constants.Query
{
    public static class Form
    {
        public static readonly string getForm = @"With CTE AS
(
SELECT 
    F.[Id] as Id,
    F.[TypeId],
    FT.TypeName,
    F.[StatusId],
    FS.StatusName,
    F.[ProcessId],
    P.ProcessName,
    F.[CustomerName],
    F.[CustomerAddress],
    F.[PhoneNumber],
    F.[PhoneNumber2],
    F.[LocationId],
    L.LocationName,
    F.[CreatedBy],
    F.[CreatedOn],
    F.[LastUpdatedBy],
    F.[LastUpdatedOn]
FROM [Form] F
inner join FormType FT on F.TypeId = FT.TypeId
inner join FormStatus FS on F.StatusId = FS.StatusId
inner join Process P on F.ProcessId = P.ProcessId
inner join Location L on F.LocationId = L.LocationId )
select count(1) OVER() AS 
TotalRecords, {2}
from CTE {0} {1}
OFFSET @offset ROWS 
FETCH NEXT @limit ROWS ONLY;";

        public static readonly string postForm = @"DECLARE @LocalDateTime DATETIME,
@LocationId int;
exec getLocalDate @LocalDateTime OUTPUT;

Select @LocationId = LocationId from Location where LocationName = @Location;

Insert into Form(TypeId, StatusId, ProcessId, CustomerName, CustomerAddress, PhoneNumber, PhoneNumber2,LocationId,CreatedBy,CreatedOn, LastUpdatedBy, LastUpdatedOn)
values
(@TypeId, @StatusId, @ProcessId, @CustomerName, @CustomerAddress,@PhoneNumber, @PhoneNumber2, @LocationId, @LastUpdatedBy, @LocalDateTime, @LastUpdatedBy, @LocalDateTime);
Select @@Identity
";

        public static readonly string updateForm = @"DECLARE @LocalDateTime DATETIME,
@LocationId int;
exec getLocalDate @LocalDateTime OUTPUT;

Select @LocationId = LocationId from Location where LocationName = @Location;
    UPDATE Form
    SET TypeId = IIF(@TypeId is NULL, TypeId, @TypeId),
        StatusId = IIF(@StatusId is NULL, StatusId, @StatusId),
        ProcessId = IIF(@ProcessId is NULL, ProcessId, @ProcessId),
        CustomerName = IIF(@CustomerName is NULL, CustomerName, @CustomerName),
        CustomerAddress = IIF(@CustomerAddress is NULL, CustomerAddress, @CustomerAddress),
        LocationId = IIF(@LocationId is NULL, LocationId, @LocationId),
        phoneNumber = IIF(@PhoneNumber is NULL, phoneNumber, @PhoneNumber),
        phoneNumber2 = IIF(@PhoneNumber2 is NULL, phoneNumber2, @PhoneNumber2),
        LastUpdatedBy = IIF(@LastUpdatedBy is NULL, LastUpdatedBy, @LastUpdatedBy),
        LastUpdatedOn =  @LocalDateTime
    WHERE Id = @Id;

";

    }
}
