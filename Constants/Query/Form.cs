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
Select @Id=@@Identity

INSERT INTO activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
VALUES (@Id, 1, '', @LastUpdatedBy, @LocalDateTime, 'Form', '', '');

Select @Id

";

        public static readonly string updateForm = @"DECLARE @LocalDateTime DATETIME,
@LocationId int,
@PrevTypeId int,
@PrevStatusId int,
@PrevProcessId int,
@PrevCustomerName varchar(100),
@PrevCustomerAddress varchar(500),
@PrevLocationId int,
@PrevPhoneNumber varchar(15),
@PrevPhoneNumber2 varchar(15);

exec getLocalDate @LocalDateTime OUTPUT;

Select @LocationId = LocationId from Location where LocationName = @Location;

Select @PrevTypeId = TypeId,
 @PrevStatusId = StatusId,
 @PrevProcessId     = ProcessId,
@PrevCustomerName = CustomerName,
@PrevCustomerAddress = CustomerAddress,
@PrevLocationId = LocationId,
@PrevPhoneNumber = PhoneNumber,
@PrevPhoneNumber2 = PhoneNumber2
from form 
where Id = @Id;

IF(@TypeId is not NULL and @TypeId <> @PrevTypeId)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Select @Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'TypeName', T1.TypeName, T.TypeName 
from FormType T inner join FormType T1 on T1.TypeId = @PrevTypeId WHERE T.TypeId = @TypeId; 
END

IF(@StatusId is not NULL and @StatusId <> @PrevStatusId)
BEGIN
IF(@StatusId = 4)
BEGIN
    Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
    Values (@Id, 3, '', @LastUpdatedBy, @LocalDateTime, 'Form', '', '');
END
ELSE
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Select @Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'StatusName', T1.StatusName, T.StatusName 
from FormStatus T inner join FormStatus T1 on T1.StatusId = @PrevStatusId WHERE T.StatusId = @StatusId; 
END
END

IF(@ProcessId is not NULL and @ProcessId <> @PrevProcessId)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Select @Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'ProcessName', T1.ProcessName, T.ProcessName 
from Process T inner join Process T1 on T1.ProcessId = @PrevProcessId WHERE T.ProcessId = @ProcessId; 
END

IF(@LocationId is not NULL and @LocationId <> @PrevLocationId)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Select @Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'LocationName', T1.LocationName, T.LocationName 
from Location T inner join Location T1 on T1.LocationId = @PrevLocationId WHERE T.LocationId = @LocationId; 
END


IF(@CustomerName is not NULL and @CustomerName <> @PrevCustomerName)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Values (@Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'CustomerName', @PrevCustomerName, @CustomerName) 
END

IF(@CustomerAddress is not NULL and @CustomerAddress <> @PrevCustomerAddress)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Values (@Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'CustomerAddress', @PrevCustomerAddress, @CustomerAddress) 
END

IF(@PhoneNumber is not NULL and @PhoneNumber <> @PrevPhoneNumber)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Values (@Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'PhoneNumber', @PrevPhoneNumber, @PhoneNumber) 
END

IF(@PhoneNumber2 is not NULL and @PhoneNumber2 <> @PrevPhoneNumber2)
BEGIN
Insert into Activity (Id, ActivityTypeId, Comments, CreatedBy, CreatedOn, Field, OldValue, NewValue)
Values (@Id, 2, '', @LastUpdatedBy, @LocalDateTime, 'PhoneNumber2', @PrevPhoneNumber2, @PhoneNumber2) 
END

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
