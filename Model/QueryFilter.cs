namespace DocumentProcessor.Model
{
    public class QueryFilter : BaseFilter
    {
        public QueryFilter(string orderBy, int? Limit = 1000, int? Offset = 0, string? Query = "", string? SortBy = "asc") : base(orderBy)
        {            
            this.Limit = Limit;
            this.Offset = Offset;
            this.Query = Query;            
            this.SortBy = SortBy;
    }        
    }
}
