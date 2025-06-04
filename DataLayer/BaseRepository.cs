using Dapper;
using DocumentProcessor.Model;

namespace DocumentProcessor.DataLayer
{
    public class BaseRepository
    {
        protected string BuildQuery(BaseFilter baseFilter, string rawQuery, ref DynamicParameters dynamicParameters)
        {
            if(string.IsNullOrEmpty(baseFilter.Field))
            {
                baseFilter.Field = baseFilter.DefaultField;
            }            
            dynamicParameters.Add("@Limit", baseFilter.Limit);
            dynamicParameters.Add("@Offset", baseFilter.Offset);
            var orderByClause = $" order by {baseFilter.OrderBy} {baseFilter.SortBy}";
            baseFilter.Filters = ParseQueryParam(baseFilter.Query);
            if (baseFilter.Filters == null || baseFilter.Filters.Count == 0)
            {
                return string.Format(rawQuery, string.Empty, orderByClause, baseFilter.Field, baseFilter.Field);
            }
            var filters = baseFilter.Filters.GroupBy(grp => grp.Target).SelectMany(trgt => trgt.Select((act, idx) => new
            {
                Left = trgt.Key,
                Right = $"{trgt.Key}{idx}",
                act.Operator,
                act.Value
            })).ToList();
            foreach (var item in filters)
            {
                if (item.Operator.Equals("bt"))
                {
                    dynamicParameters.Add($"@{item.Right}1", item.Value.Split('|').FirstOrDefault());
                    dynamicParameters.Add($"@{item.Right}2", item.Value.Split('|').LastOrDefault());
                    continue;
                }
                if (item.Operator.Equals("in") || item.Operator.Equals("ni"))
                {
                    dynamicParameters.Add($"@{item.Right}", item.Value.Split(','));
                    continue;
                }
                dynamicParameters.Add($"@{item.Right}", item.Value);
            }
            var wereClause = string.Join(" and ", filters.Select(s => GetWhereCondition(s.Operator, s.Left, s.Right)));
            var result = string.Format(rawQuery, $"where {wereClause}", orderByClause, baseFilter.Field);
            return result;
        }
        private string GetWhereCondition(string qualifier, string left, string right)
        {
            if (left.ToLower().Equals("palletid"))
            {
                return $"({string.Format(dbOperators[qualifier], left, right)} or {string.Format(dbOperators[qualifier], "fakeid", right)})";
            }
            return string.Format(dbOperators[qualifier], left, right);
        }
        private List<Filter> ParseQueryParam(string queryParam)
        {
            if (string.IsNullOrEmpty(queryParam))
            {
                return null;
            }

            return queryParam.Split(" and ").Select(val => GetFilterValue(val)).ToList();
        }
        private Filter GetFilterValue(string value)
        {
            var path = value.Trim().Split(' ');
            Filter filter = new Filter
            {
                Target = path[0],
                Operator = path[1],
                Value = string.Join(' ', path.Skip(2))
            };
            if (filter.Operator.Equals("lk"))
            {
                filter.Value = $"%{filter.Value}%";
            }

            return filter;
        }

        public static readonly Dictionary<string, string> dbOperators = new Dictionary<string, string>() {
            { "eq","{0} = @{1}"},
            { "ne","{0} <> @{1}"} ,
            { "lt","{0} < @{1}"} ,
            { "gt","{0} > @{1}"} ,
            { "le","{0} <= @{1}"} ,
            { "ge","{0} >= @{1}"} ,
            { "in","{0} in @{1}"} ,
            { "ni","{0} not in @{1}"} ,
            { "lk","{0} like @{1}"} ,
            { "bt","{0} between @{1}1 and @{1}2"} ,
        };       
    }
}
