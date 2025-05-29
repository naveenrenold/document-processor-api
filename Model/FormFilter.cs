namespace DocumentProcessor.Model
{
    public class FormFilter : BaseFilter
    {
        public FormFilter(string orderBy, int? Limit = 1000, int? Offset = 0, string? Query = "", string? SortBy = "asc") : base(orderBy)
        {            
            this.Limit = Limit;
            this.Offset = Offset;
            this.Query = Query;            
            this.SortBy = SortBy;
    }        
    }
}
