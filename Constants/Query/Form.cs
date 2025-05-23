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
    [Location],
    [CreatedBy],
    [CreatedOn],
    [LastUpdatedBy],
    [LastUpdatedOn]
FROM [Form]";
        public static readonly string postForm = @"Insert into Form values()";
    }
}
