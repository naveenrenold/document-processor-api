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
TotalRecords, Id, TypeId, TypeName, StatusId, StatusName, ProcessId, ProcessName, CustomerName, CustomerAddress, LocationId, LocationName, CreatedBy, CreatedOn, LastUpdatedBy, LastUpdatedOn
from CTE {0} {1}
OFFSET @offset ROWS 
FETCH NEXT @limit ROWS ONLY;";

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
