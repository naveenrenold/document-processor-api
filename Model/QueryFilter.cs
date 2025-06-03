namespace DocumentProcessor.Model
{
    public class QueryFilter : BaseFilter
    {
        public QueryFilter(string OrderBy, int? Limit = 1000, int? Offset = 0, string? Query = "", string? SortBy = "asc", string? Field = null) : base(OrderBy)
        {            
            this.Limit = Limit;
            this.Offset = Offset;
            this.Query = Query;            
            this.SortBy = SortBy;
            this.Field = Field;

        }        
    }
}
